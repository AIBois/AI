using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using States.Squad;
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
            float distanceToLeader = Vector3.Distance(context.transform.position, squad.Leader.transform.position);

            if (context == squad.Leader || distanceToLeader <= 5.0f)
                context.currentState = new IdleCharacterState(context);
            else 
                context.Regroup();
        }
    }
}