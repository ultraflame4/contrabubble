using UnityEngine;

namespace Player
{
    public class PointerManager : MonoBehaviour
    {
        [field: SerializeField, Tooltip("How far away the pointer should be away from the transform.parent.")]
        public float distance { get; private set; }

        public void UpdatePointer(Vector3 aimVector) 
        {
            // offset pointer from player's position + height
            transform.position = transform.parent.position + aimVector * distance;
            // Rotate the pointer to face the mouse
            transform.up = aimVector;
        }
    }
}
