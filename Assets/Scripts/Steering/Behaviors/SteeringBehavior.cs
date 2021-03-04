using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior
{
    protected const float timeToTarget = 0.1f;
    public Transform Target { get; protected set; }

    public abstract SteeringState? GetSteering(SteeringAgent agent);

    public void SetTarget(Transform transform)
    {
        Target = transform;
    }

    protected void ClampLinearAcceleration(ref SteeringState state, SteeringAgent agent)
    {
        if (state.linear.sqrMagnitude > agent.MaxAcceleration * agent.MaxAcceleration)
        {
            state.linear.Normalize();
            state.linear *= agent.MaxAcceleration;
        }
    }

    protected void ClampAngularAcceleration(ref SteeringState state, SteeringAgent agent)
    {
        var angularAccleration = Mathf.Abs(state.angular);
        if (angularAccleration > agent.MaxAngularAcceleration)
        {
            state.angular /= angularAccleration;
            state.angular *= agent.MaxAngularAcceleration;
        }
    }

}
