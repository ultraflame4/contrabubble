using System;
using UnityEngine;

namespace Player.Diver
{
    [RequireComponent(typeof(LineRenderer), typeof(Rigidbody))]
    public class DiverProjectile : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer;
        [field: SerializeField] public Rigidbody rb { get; private set; }

        public event Action<Bubble> OnCollectBubble;
        public event Action<BubbleStorage> OnHit;

        void Update()
        {
            if (lineRenderer == null) return;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.parent.position);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bubble bubble))
                OnCollectBubble?.Invoke(bubble);
            
            if (other.TryGetComponent(out BubbleStorage bubbleStorage) && bubbleStorage != this)
                OnHit?.Invoke(bubbleStorage);
        }
    }
}
