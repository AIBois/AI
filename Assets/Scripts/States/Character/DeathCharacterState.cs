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
            //assign new leader based on the max health (null if squad is dead)
            if (context == squad.Leader)
                squad.Leader = squad.Units.OrderByDescending(unit => unit.CurrentHealth).FirstOrDefault();

            squad.Units.Remove(context);
            Object.Destroy(context.gameObject);
        }
    }
}