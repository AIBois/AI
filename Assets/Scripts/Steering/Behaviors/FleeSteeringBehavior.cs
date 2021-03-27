using System.Collections;
using UnityEngine;

public class FleeSteeringBehavior : SteeringBehavior
{

    public override SteeringState? GetSteering(SteeringAgent agent, Vector3 targetPosition, float targetRotation,
        Vector3 targetVelocity)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return null;

        //Get direction
        state.linear =  agent.Position - targetPosition;
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;

        return state;
    }
}