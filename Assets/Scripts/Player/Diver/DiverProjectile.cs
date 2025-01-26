using System;
using UnityEngine;

namespace Player.Diver
{
    [RequireComponent(typeof(LineRenderer), typeof(Rigidbody))]
    public class DiverProjectile : MonoBehaviour
    {
        [SerializeField] Vector3 ropeOffset;
        [SerializeField] LineRenderer lineRenderer;
        [field: SerializeField] public Rigidbody rb { get; private set; }
        [field: SerializeField] public Collider col { get; private set; }
        [HideInInspector] public bool isFlipped = false;

        public event Action<Bubble> OnCollectBubble;
        public event Action<BubbleStorage> OnHit;

        public Vector3 startPos => transform.parent.position + GetFlippedOffset();

        void Update()
        {
            if (lineRenderer == null) return;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, startPos);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bubble bubble))
                OnCollectBubble?.Invoke(bubble);
            
            if (other.TryGetComponent(out BubbleStorage bubbleStorage) && bubbleStorage != this)
                OnHit?.Invoke(bubbleStorage);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(startPos, 0.1f);
        }

        Vector3 GetFlippedOffset()
        {
            if (!isFlipped) return ropeOffset;
            Vector3 vec = ropeOffset;
            vec.x *= -1f;
            return vec;
        }
    }
}
