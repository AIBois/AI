namespace States.Squad
{
    public class CombatSquadState : SquadState
    {
        private int startingSquadSize;
        private SquadBase enemySquad;

        public CombatSquadState(SquadBase context, SquadBase enemySquad) : base(context)
        {
            startingSquadSize = context.Units.Length;
            this.enemySquad = enemySquad;
            //set squad states to combat
        }

        protected override void Act()
        {
            if (SquadHasBeenHalved() || SquadLeaderIsDead()) 
                context.currentState = new RetreatSquadState(context, enemySquad);
        }

        private bool SquadLeaderIsDead()
        {
            return !context.Leader;
        }

        private bool SquadHasBeenHalved()
        {
            return context.Units.Length < startingSquadSize;
        }
    }
}