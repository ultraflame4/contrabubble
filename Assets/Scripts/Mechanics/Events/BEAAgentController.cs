using Player;
using Player.Diver;
using UnityEngine;

namespace Events.BEA
{
    public class BEAAgentController : MonoBehaviour
    {
        public float detectionRange = 25f;
        public float shootRange = 5f;
        public LayerMask layerMask;
        public DiverController controller;

        Collider[] cols;
        Vector3 boatPosition;
        Vector3? target = null;
        bool returnToShip = false;

        void Start()
        {
            boatPosition = transform.position;
            target = null;
            returnToShip = false;
        }

        void Update()
        {
            MoveToTarget();
            if (returnToShip) return;
            DetectTargets();
        }

        void DetectTargets()
        {
            cols = Physics.OverlapSphere(transform.position, detectionRange, layerMask);

            foreach (Collider col in cols)
            {
                if (col.TryGetComponent(out PlayerInputManager player))
                    target = player.transform.position;
                else if (col.TryGetComponent(out Bubble bubble))
                    target = bubble.transform.position;
                else
                    SearchForTarget();
            }
        }

        void SearchForTarget()
        {
            PlayerInputManager[] players = FindObjectsByType<PlayerInputManager>(FindObjectsSortMode.None);

            if (players == null || players.Length <= 0)
            {
                target = null;
                return;
            }

            foreach (PlayerInputManager player in players)
            {
                if (target == null)
                {
                    target = player.transform.position;
                    continue;
                }

                if (Vector3.Distance(player.transform.position, transform.position) >= 
                    Vector3.Distance((Vector3) target, transform.position))
                        continue;
                
                target = player.transform.position;
            }
        }

        void MoveToTarget()
        {
            if (target == null) return;

            controller._aimVector.Value = ((Vector3) target - controller.projectile.startPos).normalized;

            if (!returnToShip && Vector3.Distance(transform.position, (Vector3) target) < shootRange)
            {
                Shoot();
                return;
            }
            else
            {
                controller.OnShootHandler(false);
            }

            controller._moveInput.Value = ((Vector3) target - transform.position).normalized;
        }

        void Shoot()
        {
            if (controller.currentState != controller.Charge)
                controller.OnShootHandler(true);
            else
                controller.OnShootHandler(controller._chargeDuration.Value < controller.maxChargeDuration);
        }

        public void StartReturnToShip()
        {
            returnToShip = true;
            target = boatPosition;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootRange);
        }
    }
}
