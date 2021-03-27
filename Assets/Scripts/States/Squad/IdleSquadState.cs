using States.Character;
using UnityEngine;

namespace States.Squad
{
    public class IdleSquadState : SquadState
    {
        private new SquadBase context;

        public IdleSquadState(SquadBase context) : base(context)
        {
            context.SetUnitStates(new IdleCharacterState());
        }
        
        protected override void Act()
        {
            var closestSquad = GetClosestSquad();
            if(closestSquad) context.currentState = new MovingSquadState(context, closestSquad);
        }
        
        SquadBase GetClosestSquad()
        {
            SquadBase result = null;
            var minDist = Mathf.Infinity;
            var currentPos = transform.position;
            var enemies = FindObjectsOfType<SquadBase>();
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