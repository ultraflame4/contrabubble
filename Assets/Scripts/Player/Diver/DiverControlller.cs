using System;
using UnityEngine;
using UnityEngine.UI;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    [RequireComponent(typeof(Rigidbody))]
    public class DiverController : StateMachine<DiverController>
    {
        // inspector values
        [Header("Movement")]
        public float movementSpeed = 5f;
        public float rotationSpeed = 1f;
        [Range(0f, 1f)] public float rotationMatchThreshold = 0.99f;
        public SpriteRenderer sprite;

        [Header("Charge")]
        public float chargeDuration = 1f;
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

        #region Inputs
        [HideInInspector] public Vector3 moveInput;
        [HideInInspector] public Vector3 aimVector;
        #endregion

        #region Events
        #endregion

        #region States
        public DefaultState Default { get; private set; }
        public ChargeState Charge { get; private set; }
        public ShootState Shoot { get; private set; }
        public DrivingState Driving { get; private set; }
        #endregion

        public Rigidbody rb { get; private set; }

        public bool shootInput { get; private set; } = false;

        void Awake()
        {
            Default = new DefaultState(this, this);
            Charge = new ChargeState(this, this);
            Shoot = new ShootState(this, this);
            Driving = new DrivingState(this);
            Initialize(Default);
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }


        #region Event Listener
        public void OnShootHandler(bool input)
        {
            shootInput = input;
        }


        #endregion
    }
}
