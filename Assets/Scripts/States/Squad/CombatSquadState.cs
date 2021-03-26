using States.Character;

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
            context.SetUnitStates(new CombatCharacterState(enemySquad));
            enemySquad.IsBeingAttacked(context);
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