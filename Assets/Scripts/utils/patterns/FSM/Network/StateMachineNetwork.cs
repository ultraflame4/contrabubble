using System;
using UnityEngine;
using Unity.Netcode;

namespace Utils.Patterns.FSM
{
    public class StateMachineNetwork<T> : NetworkBehaviour
    {
        public StateNetwork<T>[] states { get; protected set; }
        public StateNetwork<T> currentState { get; protected set; }
        public string current_state_name = "None";

        /// <summary>
        /// Event callback
        /// </summary>
        /// <param name="prev">The previous state</param>
        /// <param name="next">The next state</param>
        public delegate void OnStateChanged(StateNetwork<T> prev, StateNetwork<T> next);

        /// <summary>
        /// This event is invoked when the state is changed.
        /// </summary>
        public event OnStateChanged StateChanged;

        /// <summary>
        /// Method to initialize the state machine and enter the starting state. 
        /// </summary>
        /// <param name="state">State to start in</param>
        public void Initialize(StateNetwork<T> state)
        {
            currentState = state;
            UpdateStateName();
            currentState?.Enter();
        }

        /// <summary>
        /// Method to switch states. 
        /// </summary>
        /// <param name="nextState">State to transition into</param>
        public void SwitchState(StateNetwork<T> nextState)
        {
            if (states == null)
            {
                Debug.LogError("States array is not set, unable to switch state! ");
                return;
            }

            SwitchStateRPC(Array.IndexOf(states, nextState));
        }

        /// <summary>
        /// Method to switch states. 
        /// </summary>
        /// <param name="nextStateIndex">Index of state to transition into from states array</param>
        [Rpc(SendTo.Everyone)]
        public virtual void SwitchStateRPC(int nextStateIndex)
        {   
            // set previous state
            StateNetwork<T> prev = currentState;
            // change state
            currentState?.Exit();
            currentState = states[nextStateIndex];
            UpdateStateName();
            currentState?.Enter();
            // Invoke event for side-effects.
            StateChanged?.Invoke(prev, states[nextStateIndex]);
        }

        public void UpdateStateName()
        {
            current_state_name = currentState.GetType().FullName;
        }
        
        #region Monobehaviour Callbacks
        protected void Update() 
        {
            currentState?.LogicUpdate();
        }

        protected void FixedUpdate() 
        {
            currentState?.PhysicsUpdate();
        }

        #endregion
    }
}
