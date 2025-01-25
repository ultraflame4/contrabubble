using UnityEngine;

namespace Player.Diver
{
    [RequireComponent(typeof(LineRenderer), typeof(Rigidbody))]
    public class DiverProjectile : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer;
        [field: SerializeField] public Rigidbody rb { get; private set; }

        void Update()
        {
            if (lineRenderer == null) return;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.parent.position);
        }
    }
}
