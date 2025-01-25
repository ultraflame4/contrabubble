using UnityEngine;
using Unity.Netcode;

namespace Player
{
    public class PointerManager : NetworkBehaviour
    {
        [field: SerializeField, Tooltip("How far away the pointer should be away from the transform.parent.")]
        public float distance { get; private set; }

        public void UpdatePointer() 
        {
            // ensure is owner's mouse
            if (!IsOwner) return;
            // update pointer
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 player_pos = transform.parent.position;
            // Ensure that comparison is done in 2D space
            player_pos.z = 0; 
            mouse_position.z = 0; 
            // set direction of mouse pointer to the player
            Vector3 direction = (mouse_position - player_pos).normalized;
            // offset pointer from player's position + height
            transform.position = transform.parent.position + direction * distance;
            // Rotate the pointer to face the mouse
            transform.up = direction;
        }
    }
}
