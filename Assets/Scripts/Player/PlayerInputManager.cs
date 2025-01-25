using System;
using Player.Diver;
using UnityEngine;

namespace Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        [Header("Input Settings")]
        [Range(0, 3)] public int mouseButton = 0;

        [Header("Object References")]
        public DiverController diverController;

        private Vector2 moveInput;
        private Vector2 aimVector;
        
        public event Action OnShootDown;
        public event Action OnShootUp;

        void Start()
        {
            // subscribe diver controller to shoot
            if (diverController == null) return;
            OnShootDown += diverController.Shoot;
        }
        
        void Update()
        {
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.y = Input.GetAxis("Vertical");

            if (Input.GetMouseButtonDown(mouseButton))
                OnShootDown?.Invoke();
            
            if (Input.GetMouseButtonUp(mouseButton))
                OnShootUp?.Invoke();

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
