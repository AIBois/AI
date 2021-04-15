namespace States.Squad
{
    public abstract class SquadState : State
    {
        protected SquadBase context;

        protected abstract void SetUnitStates();
        
        protected SquadState(SquadBase context)
        {
            this.context = context;
        }
    }
}