using UnityEngine;

namespace Player.Diver
{
    [RequireComponent(typeof(LineRenderer))]
    public class DiverProjectile : MonoBehaviour
    {
        LineRenderer lineRenderer;
        
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            if (lineRenderer == null) return;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.parent.position);
        }
    }
}
