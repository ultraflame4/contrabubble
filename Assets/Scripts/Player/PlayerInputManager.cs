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

        private Vector2 moveInput, aimVector;
        private Vector3 mouse_position, player_pos;

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

            // update move input
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.y = Input.GetAxis("Vertical");

            // update button event inputs
            OnShoot?.Invoke(Input.GetMouseButton(mouseButton));

            if (Input.GetKeyDown(interactKey))
                OnInteractDown?.Invoke();

            if (Input.GetKeyUp(interactKey))
                OnInteractUp?.Invoke();

            // update pointer
            mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            player_pos = transform.position;
            // Ensure that comparison is done in 2D space
            player_pos.z = 0; 
            mouse_position.z = 0; 
            // set direction of mouse pointer to the player
            aimVector = mouse_position - player_pos;

            SetDiver();
        }

        void SetDiver()
        {
            if (diverController == null || !diverController.gameObject.activeInHierarchy)
                return;

            diverController._moveInput.Value = moveInput.normalized;
            diverController._aimVector.Value = aimVector.normalized;
        }
    }
}
