namespace States
{
    public class IdleState(StateContext context) : State
    {
        private readonly StateContext context;

        public IdleState(StateContext context) : base(context) { }
        
        protected override void Act()
        {
        }

    }
}