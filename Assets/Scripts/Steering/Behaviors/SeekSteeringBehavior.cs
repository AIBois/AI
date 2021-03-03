using System.Collections;
using UnityEngine;

public class SeekSteeringBehavior : SteeringBehavior
{

    public override SteeringState GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!Target.HasValue || !agent)
            return state;

        //Get direction
        state.linear = Target.Value.TargetPosition - agent.Position;
        state.linear.Normalize();
        state.linear *= agent.MaxAcceleration;

        state.angular = 0.0f;

        return state;
    }
}