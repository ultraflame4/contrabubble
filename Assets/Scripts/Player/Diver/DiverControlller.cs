using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    [RequireComponent(typeof(Rigidbody))]
    public class DiverController : StateMachine<DiverController>
    {
        // inspector values
        public float movementSpeed = 5f;
        public float rotationSpeed = 1f;
        [Range(0f, 1f)] public float rotationMatchThreshold = 0.99f;

        #region Inputs
        [HideInInspector] public Vector3 moveInput;
        [HideInInspector] public Vector3 aimVector;
        #endregion

        #region States
        public DefaultState Default { get; private set; }
        #endregion

        public Rigidbody rb { get; private set; }

        void Awake() 
        {
            Default = new DefaultState(this, this);
            Initialize(Default);
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Shoot()
        {
            // if (currentState == Shoot) return;
            // SwitchState(Shoot);
        }
    }
}
