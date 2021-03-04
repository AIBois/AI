using System.Collections;
using UnityEngine;

public class VelocityMatchBehavior : SteeringBehavior
{
    public override SteeringState? GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!Target.HasValue || !agent)
            return null;

        state.linear = Target.Value.TargetVelocity - agent.Velocity;
        state.linear /= timeToTarget;

        ClampLinearAcceleration(ref state, agent);

        return state;
    }
}