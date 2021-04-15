using States.Character;
using UnityEngine;

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

        public override void Act()
        {
            context.MoveAwayFrom(enemySquad.transform.position);
            if(FarEnoughFromEnemy()) context.currentState = new IdleSquadState(context);
        }

        protected override void SetUnitStates()
        {
            foreach (var unit in context.Units)
                unit.currentState = new MovingCharacterState(unit);
        }

        private bool FarEnoughFromEnemy()
        {
            var distance = Vector3.Distance(enemySquad.transform.position, context.transform.position);
            return distance >= context.SafeDistance;
        }

        public void BeingAttacked(SquadBase attacker)
        {
            context.currentState = new CombatSquadState(context, enemySquad);
        }
    }
}