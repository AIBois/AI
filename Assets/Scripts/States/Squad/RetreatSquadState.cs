using States.Character;
using UnityEngine;

namespace States.Squad
{
    public class RetreatSquadState : SquadState
    {
        private SquadBase enemySquad;

        public RetreatSquadState(SquadBase context, SquadBase enemySquad) : base(context)
        {
            this.enemySquad = enemySquad;
        }

        public override void Act()
        {
            context.MoveAwayFrom(enemySquad.Leader.transform.position);
            if(FarEnoughFromEnemy()) context.currentState = new IdleSquadState(context);
        }

        protected override void SetUnitStates()
        {
            foreach (var unit in context.Units)
                unit.currentState = new MovingCharacterState(unit);
        }

        private bool FarEnoughFromEnemy()
        {
            var distance = Vector3.Distance(enemySquad.Leader.transform.position, context.Leader.transform.position);
            return distance >= context.SafeDistance;
        }
    }
}