using System.Linq;
using States.Character;
using UnityEngine;

namespace States.Squad
{
    public class RegroupSquadState : SquadState
    {
        private const float TargetCohesion = 3.0f;

        public RegroupSquadState(SquadBase context) : base(context)
        {
        }

        public override void Act()
        {
            if (SquadIsCloseEnoughTogether())
                context.currentState = new IdleSquadState(context);
            else
                SetUnitStates();
        }

        private bool SquadIsCloseEnoughTogether()
        {
            return GetCohesionValue() < TargetCohesion;
        }

        private float GetCohesionValue()
        {
            var distanceToLeader = context.Units.Where(characterBase => context.Leader != characterBase)
                .Sum(characterBase => Vector3.Distance(characterBase.transform.position, context.Leader.transform.position));
            return distanceToLeader / context.Units.Count;
        }

        protected override void SetUnitStates()
        {
            foreach (var unit in context.Units)
                unit.currentState = new RegroupCharacterState(unit);
        }
    }
}