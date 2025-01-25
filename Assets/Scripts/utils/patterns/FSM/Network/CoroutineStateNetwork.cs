using UnityEngine;

namespace Utils.Patterns.FSM
{
    public class CoroutineStateNetwork<T> : StateNetwork<T>
    {
        private Coroutine coroutine;
        protected StateNetwork<T> nextState;
        protected float duration;

        /// <summary>
        /// Constructor to create a state that automatically transitions to another state after a set duration
        /// </summary>
        /// <param name="fsm">Reference to state machine of type StateMachineNetwork</param>
        /// <param name="character">Reference to character of generic type</param>
        /// <param name="nextState">State to transition to after set duration</param>
        /// <param name="duration">Duration to wait before transitioning to next state</param>
        public CoroutineStateNetwork(StateMachineNetwork<T> fsm, T character, StateNetwork<T> nextState, float duration) : base(fsm, character)
        {
            this.nextState = nextState;
            this.duration = duration;
        }

        public override void Enter()
        {
            base.Enter();
            // start coroutine to count state duration
            coroutine = fsm.StartCoroutine(WaitForSeconds(duration, () => 
                {
                    coroutine = null;
                    fsm.SwitchState(nextState);
                }
            ));
        }

        public override void Exit() 
        {
            base.Exit();
            // stop coroutine when changing state
            if (coroutine == null) return;
            fsm.StopCoroutine(coroutine);
        }
    }
}
