using System.Collections;
using UnityEngine;

public class SeekSteeringBehavior : SteeringBehavior
{

    public override SteeringState? GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!Target.HasValue || !agent)
            return null;

        //Get direction
        state.linear = Target.Value.TargetPosition - agent.Position;
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;

        return state;
    }
}