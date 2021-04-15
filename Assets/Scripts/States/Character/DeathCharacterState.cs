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
            squad.Units.Remove(context);
            Object.Destroy(context.gameObject);
        }
    }
}