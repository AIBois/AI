namespace States.Squad
{
    public class RetreatSquadState : SquadState, IAttackListener
    {
        private SquadBase enemySquad;

        public RetreatSquadState(SquadBase context, SquadBase enemySquad) : base(context)
        {
            this.enemySquad = enemySquad;
            context.attackListener = this;
        }

        protected override void Act()
        {
            context.MoveAwayFrom(enemySquad.transform.position);
            if(FarEnoughFromEnemy()) context.currentState = new IdleSquadState(context);
        }

        private bool FarEnoughFromEnemy()
        {
            throw new System.NotImplementedException();
        }

        public void BeingAttacked()
        {
            context.currentState = new CombatSquadState(context, enemySquad);
        }
    }
}