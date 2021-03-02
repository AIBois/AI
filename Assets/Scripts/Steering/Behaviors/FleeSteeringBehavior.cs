using System.Collections;
using UnityEngine;

public class FleeSteeringBehavior : SteeringBehavior
{

    public override SteeringState GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!TargetPosition.HasValue || !agent)
            return state;

        //Get direction
        state.linear =  agent.Position - TargetPosition.Value;
        state.linear.Normalize();
        state.linear *= agent.MaxAcceleration;

        state.angular = 0.0f;

        return state;
    }
}