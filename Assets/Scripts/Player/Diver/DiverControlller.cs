using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    [RequireComponent(typeof(Rigidbody))]
    public class DiverController : StateMachineNetwork<DiverController>
    {
        // inspector values
        [Header("Movement")]
        public float movementSpeed = 5f;
        public float rotationSpeed = 1f;
        [Range(0f, 1f)] public float rotationMatchThreshold = 0.99f;
        public SpriteRenderer sprite;

        [Header("Charge")]
        public float maxChargeDuration = 1f;
        public Slider chargeSlider;

        [Header("Shoot")]
        public float shootDuration = 1f;
        public float minShootForce = 50f;
        public float maxShootForce = 150f;
        public float pullbackSpeed = 2f;
        public float pullbackStopDistance = 0.1f;
        [Range(0f, 1f)] public float pullbackWindow = 0.3f;
        public PointerManager pointer;
        public DiverProjectile projectile;

        #region States
        public DefaultState Default { get; private set; }
        public ChargeState Charge { get; private set; }
        public ShootState Shoot { get; private set; }
        #endregion

        #region Inputs
        public Vector3 moveInput => _moveInput.Value;
        public Vector3 aimVector => _aimVector.Value;
        public bool shootInput => _shootInput.Value;
        #endregion

        #region Others
        public float chargeDuration
        {
            get { return _chargeDuration.Value; }
            set 
            {
                if (!IsOwner) return;
                _chargeDuration.Value = value; 
            }
        }
        #endregion

        #region Network Variables
        // inputs
        [HideInInspector] public NetworkVariable<Vector3> _moveInput = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public NetworkVariable<Vector3> _aimVector = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public NetworkVariable<bool> _shootInput = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        // others
        [HideInInspector] public NetworkVariable<float> _chargeDuration = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        #endregion

        public Rigidbody rb { get; private set; }

        public IDriveableVehicle availableVehicle;

        void Awake()
        {
            Default = new DefaultState(this);
            Charge = new ChargeState(this);
            Shoot = new ShootState(this);

            // apply all states into states array
            states = new StateNetwork<DiverController>[]
            {
                Default, Charge, Shoot
            };

            // initialize and enter first state to start fsm
            Initialize(Default);
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        #region Event Listener
        public void OnShootHandler(bool input)
        {
            _shootInput.Value = input;
        }
        #endregion
    }
}
