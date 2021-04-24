using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSteeringBehavior : AlighBehavior
{
    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents = null)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return state;

        Vector3 direction = target.GetPosition() - agent.Position;

        if (Mathf.Abs(direction.sqrMagnitude) <= float.Epsilon)
            return state;

        target.Rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        return base.GetSteering(agent, target, groupAgents);
    }
}
