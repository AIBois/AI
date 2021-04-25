using States.Character;

namespace States.Squad
{
    public class CombatSquadState : SquadState
    {
        private int startingSquadSize;
        private SquadBase enemySquad;

        public CombatSquadState(SquadBase context, SquadBase enemySquad) : base(context)
        {
            startingSquadSize = context.Units.Count;
            this.enemySquad = enemySquad;
            SetUnitStates();
            context.MoveTo(context.GetAveragedPosition());
        }

        protected override void SetUnitStates()
        {
            foreach (var unit in context.Units)
                unit.currentState = new CombatCharacterState(unit, enemySquad);
        }

        public override void Act()
        {
            if ((SquadHasBeenHalved() || SquadLeaderIsDead()) && SquadGreaterThanOne())
                context.currentState = new RetreatSquadState(context, enemySquad);
        }

        private bool SquadLeaderIsDead()
        {
            return !context.Leader;
        }

        private bool SquadHasBeenHalved()
        {
            return context.Units.Count < startingSquadSize;
        }

        private bool SquadGreaterThanOne()
        {
            return context.Units.Count > 1;
        }

    }
}