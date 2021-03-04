using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlighBehavior : SteeringBehavior
{
    public override SteeringState? GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!Target.HasValue || !agent)
            return null;

        float distanceToTarget = Vector3.Distance(agent.Position, Target.Value.TargetPosition);
        float rotation =
            Mathf.DeltaAngle(agent.Orientation.eulerAngles.y, Target.Value.TargetOrientation.eulerAngles.y);
        float absRotation = Mathf.Abs(rotation);
        float absDistance = Mathf.Abs(distanceToTarget);
        float timeToTarget = 0.1f;

        float targetRotation;
        if (distanceToTarget > agent.SlowRadius)
        {
            targetRotation = agent.MaxRotation;
        }
        else
        {
            targetRotation = agent.MaxRotation * absRotation / agent.StopRadius;
        }

        targetRotation *= rotation;

        state.angular = targetRotation - agent.rotation;
        state.angular /= timeToTarget;

        var angularAccleration = Mathf.Abs(state.angular);
        if (angularAccleration > agent.MaxAngularAcceleration)
        {
            state.angular /= angularAccleration;
            state.angular *= agent.MaxAngularAcceleration;
        }

        return state;
    }
}