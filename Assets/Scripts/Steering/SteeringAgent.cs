﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    #region Member vars
    [SerializeField]
    private float maxAcceeration = 10.0f;
    [SerializeField]
    private float maxSpeed = 15.0f;
    [SerializeField]
    private float slowRadius = 7.5f;
    [SerializeField]
    private float stopRadius = 1.5f;

    [SerializeField]
    private SteeringBehaviorType steeringType = SteeringBehaviorType.SEEK;
    [SerializeField]
    private Transform target; //This is just for testing
    #endregion

    #region properties
    public float MaxAcceleration
    {
        get => maxAcceeration;
        set => maxAcceeration = value;
    }
    public float MaxSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }
    public float SlowRadius
    {
        get => slowRadius;
        set => slowRadius = value;
    }
    public float StopRadius
    {
        get => stopRadius;
        set => stopRadius = value;
    }
    public SteeringBehaviorType SteeringType
    {
        get => steeringType;
        set => steeringType = value;
    }
    public Transform Target
    {
        get => target;
        set => target = value;
    }

    public SteeringBehavior CurrentSteeringBehavior { get; private set; }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public Vector3 Velocity { get; private set; }
    public Quaternion Orientation
    {
        get => transform.rotation;
        set => transform.rotation = value;
    }
    public float rotation { get; private set; }
    #endregion

    void Awake()
    {
        CurrentSteeringBehavior = SteeringBehaviorFactory.Create(SteeringType);
        Position = transform.position;
    }

    public void IntegrateSteering(SteeringState steering)
    {
        Velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        Debug.Log(Velocity);

        if (Velocity.sqrMagnitude > MaxSpeed * MaxSpeed)
            Velocity = Velocity.normalized * MaxSpeed;

        Position += Velocity * Time.deltaTime;
        transform.position = Position;
    }

    public void Update()
    {
        if(CurrentSteeringBehavior == null)
            return;

        //Set Target;
        CurrentSteeringBehavior.SetTarget(Target);
        var steeringInfo = CurrentSteeringBehavior.GetSteering(this);

        IntegrateSteering(steeringInfo);
    }
}