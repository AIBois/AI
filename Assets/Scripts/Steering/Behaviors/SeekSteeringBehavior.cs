using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekSteeringBehavior : SteeringBehavior
{

    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents = null)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return state;

        //Get direction
        state.linear = target.GetPosition() - agent.Position;
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;

        return state;
    }
}