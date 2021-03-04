using System.Collections;
using UnityEngine;

public class FleeSteeringBehavior : SteeringBehavior
{

    public override SteeringState? GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!Target.HasValue || !agent)
            return null;

        //Get direction
        state.linear =  agent.Position - Target.Value.TargetPosition;
        state.linear.Normalize();
        state.linear *= agent.MaxAcceleration;

        state.angular = 0.0f;

        return state;
    }
}