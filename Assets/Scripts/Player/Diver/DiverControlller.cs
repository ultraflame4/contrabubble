using System;
using UnityEngine;
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

        [Header("Shoot")]
        public float shootDuration = 1f;
        public PointerManager pointer;
        public DiverProjectile projectile;

        #region Inputs
        [HideInInspector] public Vector3 moveInput;
        [HideInInspector] public Vector3 aimVector;
        #endregion

        #region States
        public DefaultState Default { get; private set; }
        public ShootState Shoot { get; private set; }
        #endregion

        public Rigidbody rb { get; private set; }
        public bool shootInput { get; private set; } = false;

        void Awake() 
        {
            Default = new DefaultState(this, this);
            Shoot = new ShootState(this, this);
            Initialize(Default);
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void LateUpdate() 
        {
            shootInput = false;
        }

        public void OnShootHandler()
        {
            shootInput = true;
        }
    }
}
