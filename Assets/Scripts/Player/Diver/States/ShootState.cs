using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class ShootState : CoroutineState<DiverController>
    {
        public ShootState (StateMachine<DiverController> fsm, DiverController character) : base (fsm, character, character.Default, character.shootDuration)
        {
        }

        public override void Enter() 
        {
            base.Enter();
            character.projectile.gameObject.SetActive(true);
        }

        public override void Exit() 
        {
            base.Exit();
            character.projectile.gameObject.SetActive(false);
        }
    }
}

