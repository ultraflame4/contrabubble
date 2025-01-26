using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class StunState : CoroutineStateNetwork<DiverController>
    {
        public StunState(DiverController fsm) : base(fsm, fsm, fsm.Default, fsm.stunDuration)
        {
        }

        public override void Enter()
        {
            duration = character.stunDuration;
            base.Enter();
        }
    }
}

