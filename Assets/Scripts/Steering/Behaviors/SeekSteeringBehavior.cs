using System.Collections;
using UnityEngine;

public class SeekSteeringBehavior : SteeringBehavior
{

    public override SteeringState? GetSteering(SteeringAgent agent, Vector3 targetPosition, float targetRotation,
        Vector3 targetVelocity)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return null;

        //Get direction
        state.linear = targetPosition - agent.Position;
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;

        return state;
    }
}