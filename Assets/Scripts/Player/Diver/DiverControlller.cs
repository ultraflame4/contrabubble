using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    [RequireComponent(typeof(Rigidbody), typeof(BubbleStorage))]
    public class DiverController : StateMachineNetwork<DiverController>
    {
        // inspector values
        [Header("Movement")]
        public float movementSpeed = 5f;
        public float rotationSpeed = 1f;
        [Range(0f, 1f)] public float rotationMatchThreshold = 0.99f;
        public SpriteRenderer sprite;

        [Header("Shoot")]
        public float shootDuration = 1f;
        public float maxChargeDuration = 1f;
        public float minShootForce = 50f;
        public float maxShootForce = 150f;
        public float pullbackSpeed = 2f;
        public float pullbackStopDistance = 0.1f;
        [Range(0f, 1f)] public float pullbackWindow = 0.3f;
        public PointerManager pointer;
        public VehiclePassenger vehiclePassenger;
        public DiverProjectile projectile;

        [Header("UI")]
        public Canvas canvas;
        public Slider chargeSlider;

        #region States
        public DefaultState Default { get; private set; }
        public ChargeState Charge { get; private set; }
        public ShootState Shoot { get; private set; }
        public DrivingState Driving { get; private set; }
        #endregion

        #region Inputs
        public Vector3 moveInput => _moveInput.Value;
        public Vector3 aimVector => _aimVector.Value;
        public bool shootInput => _shootInput.Value;
        #endregion

        #region Others
        public float chargeDuration {
            get { return _chargeDuration.Value; }
            set {
                if (!IsOwner) return;
                _chargeDuration.Value = value;
            }
        }

        public float timeInShoot {
            get { return _timeInShoot.Value; }
            set {
                if (!IsOwner) return;
                _timeInShoot.Value = value;
            }
        }

        public bool shootHit {
            get { return _shootHit.Value; }
            set {
                if (!IsOwner) return;
                _shootHit.Value = value;
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
        [HideInInspector] public NetworkVariable<float> _timeInShoot = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public NetworkVariable<bool> _shootHit = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        #endregion

        public Rigidbody rb { get; private set; }
        public BubbleStorage bubbleStorage { get; private set; }

        void Awake()
        {
            Default = new DefaultState(this);
            Charge = new ChargeState(this);
            Shoot = new ShootState(this);
            Driving = new DrivingState(this);

            // apply all states into states array
            states = new StateNetwork<DiverController>[]
            {
                Default, Charge, Shoot, Driving
            };

            if (!vehiclePassenger) TryGetComponent(out vehiclePassenger);
            vehiclePassenger.EnteredVehicle += OnEnterVehicle;
            vehiclePassenger.ExitedVehicle += OnExitVehicle;


            // initialize and enter first state to start fsm
            Initialize(Default);
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            bubbleStorage = GetComponent<BubbleStorage>();
        }

        new void Update()
        {
            base.Update();
            // ensure canvas is always facing up
            if (canvas == null) return;
            canvas.transform.up = Vector3.up;
        }

        #region Event Listener
        public void OnEnterVehicle(bool isDriver)
        {
            SwitchState(Driving);
        }
        public void OnExitVehicle()
        {
            SwitchState(Default);
        }

        public void OnShootHandler(bool input)
        {
            _shootInput.Value = input;
        }
        #endregion
    }
}
