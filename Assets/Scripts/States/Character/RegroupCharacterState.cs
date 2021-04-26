using UnityEngine;

namespace States.Character
{
    public class RegroupCharacterState : CharacterState
    {

        public RegroupCharacterState(CharacterBase context) : base(context)
        {
        }

        public override void Act()
        {
            var squad = context.transform.parent.GetComponent<SquadBase>();
            var distanceToLeader = Vector3.Distance(context.transform.position, squad.Leader.transform.position);
            if (context == squad.Leader || distanceToLeader <= 5.0f)
                context.currentState = new IdleCharacterState(context);
            else 
                context.Regroup();
        }
    }
}