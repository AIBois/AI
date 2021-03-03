using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior
{
    public Vector3? TargetPosition { get; set; }
    public Vector3? TargetVelocity { get; set; }
    public float? TargetRotation { get; set; }

    public abstract SteeringState GetSteering(SteeringAgent agent);

}
