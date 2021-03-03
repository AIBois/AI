using UnityEngine;

namespace States.Squad
{
    public class MovingSquadState : SquadState
    {
        private SquadBase closestSquad;
        
        public MovingSquadState(SquadBase context, SquadBase closestSquad) : base(context)
        {
            this.closestSquad = closestSquad;
        }

        protected override void Act()
        {
            context.MoveTo(closestSquad.transform.position);
            if (EnemyWithinAttackingDistance()) context.currentState = new CombatSquadState(context, closestSquad);
        }

        private bool EnemyWithinAttackingDistance()
        {
            return Vector3.Distance(closestSquad.transform.position, transform.position) <= context.Leader.AttackDistance;
        }
    }
}