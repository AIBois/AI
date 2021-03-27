using System.Collections;
using UnityEngine;

public class VelocityMatchBehavior : SteeringBehavior
{
    public override SteeringState? GetSteering(SteeringAgent agent, Vector3 targetPosition, float targetRotation,
        Vector3 targetVelocity)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return null;

        state.linear = targetVelocity - agent.Velocity;
        state.linear /= timeToTarget;

        ClampLinearAcceleration(ref state, agent);

        return state;
    }
}