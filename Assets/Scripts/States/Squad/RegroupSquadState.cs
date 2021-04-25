using States.Character;
using UnityEngine;

namespace States.Squad
{
    public class RegroupSquadState : SquadState
    {
        private const float TargetCohesion = 3.0f;

        public RegroupSquadState(SquadBase context) : base(context)
        {
            SetUnitStates();
        }

        public override void Act()
        {
            if (getCohesionValue() < TargetCohesion)
                context.currentState = new IdleSquadState(context);
            else
                foreach (var unit in context.Units)
                    unit.currentState = new RegroupCharacterState(unit);
        }

        protected override void SetUnitStates()
        {

        }

        private float getCohesionValue()
        {
            float distToLeaderSum = 0.0f;

            foreach (var characterBase in context.Units)
            {
                if(context.Leader == characterBase)
                    continue;

                distToLeaderSum += Vector3.Distance(characterBase.transform.position, context.Leader.transform.position);
            }

            return distToLeaderSum / context.Units.Count;
        }

    }
}