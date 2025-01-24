using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class DiverController : StateMachine<DiverController>
    {
        #region States
        public IdleState Idle { get; private set; }
        #endregion

        void Awake() 
        {
            Idle = new IdleState(this, this);
            Initialize(Idle);
        }
    }
}
