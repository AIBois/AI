using States.Character;
using UnityEngine;

namespace States.Squad
{
    public class IdleSquadState : SquadState
    {
        public IdleSquadState(SquadBase context) : base(context)
        {
            SetUnitStates();
        }

        public override void Act()
        {
            var closestSquad = GetClosestSquad();
            if(closestSquad) context.currentState = new MovingSquadState(context, closestSquad);
        }

        protected override void SetUnitStates()
        {
            foreach (var unit in context.Units)
                unit.currentState = new IdleCharacterState(unit);
        }

        SquadBase GetClosestSquad()
        {
            SquadBase result = null;
            var minDist = Mathf.Infinity;
            var currentPos = context.transform.position;
            var enemies = Object.FindObjectsOfType<SquadBase>();
            foreach (var enemy in enemies)
            {
                var dist = Vector3.Distance(enemy.transform.position, currentPos);
                if (!(dist < minDist)) continue;
                result = enemy;
                minDist = dist;
            }
            return result;
        }
    }
}