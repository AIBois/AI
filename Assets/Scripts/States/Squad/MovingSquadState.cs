using States.Character;
using UnityEngine;

namespace States.Squad
{
    public class MovingSquadState : SquadState, IAttackListener
    {
        private SquadBase closestSquad;
        
        public MovingSquadState(SquadBase context, SquadBase closestSquad) : base(context)
        {
            this.closestSquad = closestSquad;
            SetUnitStates();
        }

        protected override void SetUnitStates()
        {
            foreach (var unit in context.Units)
                unit.currentState = new MovingCharacterState(unit);
        }
        
        public override void Act()
        {
            if (!closestSquad)
            {
                context.currentState = new IdleSquadState(context);
                return;
            }
            context.MoveTo(closestSquad.Leader);
            if (EnemyWithinAttackingDistance()) 
                context.currentState = new CombatSquadState(context, closestSquad);
        }

        private bool EnemyWithinAttackingDistance()
        {
            var distance = Vector3.Distance(closestSquad.Leader.transform.position, context.Leader.transform.position);
            return context.Leader.IsRanged
                ? distance <= context.Leader.RangedAttackLongDistance
                : distance <= context.Leader.MeleeAttackDistance;
        }

        public void BeingAttacked(SquadBase attacker)
        {
            context.currentState = new CombatSquadState(context, attacker);
        }
    }
}