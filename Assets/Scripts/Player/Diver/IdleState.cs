using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class IdleState : State<DiverController>
    {
        public IdleState (StateMachine<DiverController> fsm, DiverController character) : base (fsm, character)
        {
        }
    }
}
