using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class DrivingState : StateNetwork<DiverController>
    {

        
        Submarine submarine;
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
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (character.vehiclePassenger.is_driver) {
                submarine.AccelerateRpc(character.moveInput, 5);
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
