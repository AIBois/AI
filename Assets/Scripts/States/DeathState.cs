namespace States
{
    public class DeathState : State
    {
        public DeathState(StateContext context) : base(context) { }
        
        protected override void Act()
        {
            throw new System.NotImplementedException();
        }
    }
}