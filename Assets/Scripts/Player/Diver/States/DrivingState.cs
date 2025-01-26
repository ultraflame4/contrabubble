using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class DrivingState : StateNetwork<DiverController>
    {

        float baseDriveSpeed = 1;
        float speedMultiplier = 1;
        Submarine submarine;
        float cooldown_secs = 0.1f;
        float cooldown_counter = 0;
        public DrivingState(DiverController fsm) : base(fsm, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.collider.enabled = false;
            character.sprite.enabled = false;
            character.rb.linearVelocity = Vector3.zero;
            character.pointer.gameObject.SetActive(false);
            submarine = character.vehiclePassenger.submarine;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!character.IsOwner) return;
            if (character.shootInput && cooldown_counter <= 0) {
                submarine.ShootProjectileRpc(character.aimVector.normalized * 8);
                cooldown_counter = cooldown_secs;
            }
            if (cooldown_counter > 0) {
                cooldown_counter -= Time.deltaTime;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (character.vehiclePassenger.is_driver) {
                submarine.AccelerateRpc(character.moveInput, baseDriveSpeed * speedMultiplier);
            }
        }

        public override void Exit()
        {
            base.Exit();
            character.collider.enabled = true;
            character.sprite.enabled = true;
            character.pointer.gameObject.SetActive(true);
            character.rb.linearVelocity = Vector3.zero;
            character.rb.MovePosition(submarine.doorMarker.position);
        }


    }
}
