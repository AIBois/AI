using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlighBehavior : SteeringBehavior
{
    public override SteeringState? GetSteering(SteeringAgent agent, SteeringTarget target)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return null;

        float distanceToTarget = Vector3.Distance(agent.Position, target.Position);
        float rotation =
            Mathf.DeltaAngle(agent.Orientation.eulerAngles.y, target.Rotation);
        float absRotation = Mathf.Abs(rotation);

        float agentRotation;
        if (distanceToTarget > agent.SlowRadius)
        {
            agentRotation = agent.MaxRotation;
        }
        else
        {
            agentRotation = agent.MaxRotation * absRotation / agent.StopRadius;
        }

        agentRotation *= rotation;

        state.angular = agentRotation - agent.rotation;
        state.angular /= timeToTarget;

        ClampAngularAcceleration(ref state,agent);

        return state;
    }
}