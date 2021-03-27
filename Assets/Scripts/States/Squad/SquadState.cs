namespace States.Squad
{
    public abstract class SquadState : State
    {
        public SquadBase context;

        protected SquadState(SquadBase context) { }
    }
}