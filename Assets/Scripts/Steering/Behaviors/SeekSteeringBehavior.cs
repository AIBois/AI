using System.Collections;
using UnityEngine;

public class SeekSteeringBehavior : SteeringBehavior
{

    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return state;

        //Get direction
        state.linear = target.Position - agent.Position;
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;

        return state;
    }
}