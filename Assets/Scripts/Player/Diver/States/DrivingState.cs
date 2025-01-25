using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class DrivingState : State<DiverController>
    {

        public IDriveableVehicle vehicle;
        public bool is_driver;
        public DrivingState(DiverController fsm) : base(fsm, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.sprite.enabled = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (is_driver) {
                vehicle.Accelerate(character.moveInput * 50 * Time.deltaTime, 5);
            }

        }

        public override void Exit()
        {
            base.Exit();
            character.sprite.enabled = true;

        }
    }
}

