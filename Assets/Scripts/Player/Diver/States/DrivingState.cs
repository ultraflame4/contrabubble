using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class DrivingState : StateNetwork<DiverController>
    {
    

        public DrivingState(DiverController fsm) : base (fsm, fsm)
        {
        }

        public override void LogicUpdate() 
        {
            base.LogicUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (character.vehiclePassenger.is_driver){
                character.vehiclePassenger.submarine!.AccelerateRpc(character.moveInput, 5);
            }
        }

        public override void Exit()
        {
            base.Exit();
            
        }

    
    }
}
