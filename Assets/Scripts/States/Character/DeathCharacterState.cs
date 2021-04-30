using System.Linq;
using UnityEngine;

namespace States.Character
{
    public class DeathCharacterState : CharacterState
    {
        private readonly SquadBase ownSquad;
        private readonly SquadTypes enemySquadType;

        public DeathCharacterState(CharacterBase context, SquadBase ownSquad, SquadTypes enemySquadType) : base(context)
        {
            this.ownSquad = ownSquad;
            this.enemySquadType = enemySquadType;
        }
        
        public override void Act()
        {
            AssignNewLeader();
            ownSquad.learningData.numKilledBySquadType[(int)enemySquadType]++;
            ownSquad.Units.Remove(context);
            Object.Destroy(context.gameObject);
            if (SquadIsEmpty())
            {
                Object.FindObjectOfType<LearningCumulativeData>().AddSquadData(ownSquad.squadType, ownSquad.learningData);
                Object.FindObjectOfType<WinTracker>().removeSquad(ownSquad);
                Object.Destroy(ownSquad.gameObject);
            }
        }

        private void AssignNewLeader()
        {
            if (context == ownSquad.Leader)
                ownSquad.Leader = ownSquad.Units.OrderByDescending(unit => unit.CurrentHealth).FirstOrDefault();
        }

        private bool SquadIsEmpty()
        {
            return ownSquad.Units.Count == 0;
        }
    }
}