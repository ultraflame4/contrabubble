using System;
using UnityEngine;

namespace Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        [Range(0, 3)] public int mouseButton = 0;

        private Vector2 moveInput;
        private Vector2 aimVector;
        
        public event Action OnShootDown;
        public event Action OnShootUp;
        
        void Update()
        {
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.y = Input.GetAxis("Vertical");

            if (Input.GetMouseButtonDown(mouseButton))
                OnShootDown?.Invoke();
            
            if (Input.GetMouseButtonUp(mouseButton))
                OnShootUp?.Invoke();
        }
    }
}
