using System.Collections;
using UnityEngine;

public class VelocityMatchBehavior : SteeringBehavior
{
    public override SteeringState? GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!agent || !agent.Target)
            return null;

        var targetAgent = agent.Target.GetComponent<SteeringAgent>();
        if (!targetAgent)
            return null;

        state.linear = targetAgent.Velocity - agent.Velocity;
        state.linear /= timeToTarget;

        ClampLinearAcceleration(ref state, agent);

        return state;
    }
}