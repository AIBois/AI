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
    //If character base is set it will move to the character and ignore the pos,vel,rotation
    public CharacterBase CharacterBase;

    private Vector3 Position;
    public Vector3 Velocity;
    public float Rotation;

    public void SetPosition(Vector3 pos)
    {
        Position = pos;
    }

    public Vector3 GetPosition()
    {
        return CharacterBase ? CharacterBase.transform.position : Position;
    }
}
