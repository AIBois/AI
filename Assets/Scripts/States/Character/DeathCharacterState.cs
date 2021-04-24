using System.Linq;
using UnityEngine;

namespace States.Character
{
    public class DeathCharacterState : CharacterState
    {
        private readonly SquadBase squad;

        public DeathCharacterState(CharacterBase context, SquadBase squad) : base(context)
        {
            this.squad = squad;
        }
        
        public override void Act()
        {
            AssignNewLeader();
            squad.Units.Remove(context);
            Object.Destroy(context.gameObject);
            if(SquadIsEmpty()) Object.Destroy(squad.gameObject);
        }

        private void AssignNewLeader()
        {
            if (context == squad.Leader)
                squad.Leader = squad.Units.OrderByDescending(unit => unit.CurrentHealth).FirstOrDefault();
        }

        private bool SquadIsEmpty()
        {
            return squad.Units.Count == 0;
        }
    }
}