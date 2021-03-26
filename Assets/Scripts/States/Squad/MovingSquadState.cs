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
            context.SetUnitStates(new MovingCharacterState());
        }

        protected override void Act()
        {
            context.MoveTo(closestSquad.transform.position);
            if (EnemyWithinAttackingDistance()) context.currentState = new CombatSquadState(context, closestSquad);
        }

        private bool EnemyWithinAttackingDistance()
        {
            var distance = Vector3.Distance(closestSquad.transform.position, transform.position);
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