using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeSteeringBehavior : SteeringBehavior
{

    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents = null)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return state;

        //Get direction
        state.linear =  agent.Position - target.GetPosition();
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;

        return state;
    }
}