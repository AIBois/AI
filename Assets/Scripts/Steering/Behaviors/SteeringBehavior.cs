using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SteeringTarget
{
    public Vector3 TargetPosition { get; set; }
    public Vector3 TargetVelocity { get; set; }
    public Quaternion TargetOrientation { get; set; }
}

public abstract class SteeringBehavior
{
    protected const float timeToTarget = 0.1f;
    public SteeringTarget? Target { get; protected set; }

    public abstract SteeringState? GetSteering(SteeringAgent agent);

    public void SetTarget(SteeringTarget target)
    {
        Target = target;
    }

    public void SetTarget(Transform transform)
    {
        Target = new SteeringTarget {TargetPosition = transform.position, TargetOrientation = transform.rotation};
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
