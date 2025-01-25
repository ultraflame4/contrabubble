using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class DrivingState : StateNetwork<DiverController>
    {

        public IDriveableVehicle vehicle;
        public bool is_driver;
        public DrivingState(DiverController fsm) : base(fsm, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (is_driver)
            {
                vehicle.MoveDirection(character.moveInput);
            }

        }

        public override void Exit()
        {
            base.Exit();

        }
    }
}

