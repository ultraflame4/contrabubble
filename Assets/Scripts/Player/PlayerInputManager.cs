using System;
using Player.Diver;
using UnityEngine;
using Unity.Netcode;

namespace Player
{
    public class PlayerInputManager : NetworkBehaviour
    {
        [Header("Input Settings")]
        public KeyCode interactKey = KeyCode.E;
        [Range(0, 3)] public int mouseButton = 0;

        [Header("Object References")]
        public DiverController diverController;

        private Vector2 moveInput;
        private Vector2 aimVector;

        public event Action<bool> OnShoot;
        public event Action OnInteractDown;
        public event Action OnInteractUp;

        void Start()
        {
            // subscribe diver controller to shoot
            if (diverController == null) return;
            OnShoot += diverController.OnShootHandler;
        }

        void Update()
        {
            if (!IsOwner) return;

            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.y = Input.GetAxis("Vertical");

            OnShoot?.Invoke(Input.GetMouseButton(mouseButton));

            if (Input.GetKeyDown(interactKey))
                OnInteractDown?.Invoke();

            if (Input.GetKeyUp(interactKey))
                OnInteractUp?.Invoke();

            SetDiver();
        }

        void SetDiver()
        {
            if (diverController == null || !diverController.gameObject.activeInHierarchy)
                return;

            diverController.moveInput = moveInput.normalized;
            diverController.aimVector = aimVector.normalized;
        }
    }
}
