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

}
