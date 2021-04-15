using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatchBehavior : SteeringBehavior
{
    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents = null)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return state;

        state.linear = target.Velocity - agent.Velocity;
        state.linear /= timeToTarget;

        ClampLinearAcceleration(ref state, agent);

        return state;
    }
}