using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SteeringState
{
    public Vector3 linear;
    public float angular;
}

public struct SteeringTarget
{
    public Vector3 Position;
    public Vector3 Velocity;
    public float Rotation;
}
