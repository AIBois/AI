﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBase : MonoBehaviour
{
    public CharacterBase Leader;
    public List<CharacterBase> Units;
    [SerializeField]
    private float cost;

    private void Awake()
    {
        //TODO:: create a randomisation of the cost based ont he individual units.

        foreach (var unit in Units)
        {
            SteeringAgent agent = unit.GetComponent<SteeringAgent>();
            if (agent)
            {
                agent.Squad = this;
            }
        }
    }

    void Update()
    {
        
    }

    //public SteeringTarget GetAveragedPosition()
    //{
    //    Vector3 averagePos = Vector3.zero;
    //    Vector3 averageVelocity = Vector3.zero;
    //    float averageRotation = 0.0f;

    //    foreach (var unit in Units)
    //    {
    //        averagePos += unit.transform.position;
    //        //TODO: average velocity calculation
    //        averageRotation += unit.transform.rotation.eulerAngles.y;
    //    }

    //    averagePos /= Units.Count;
    //    averageRotation /= Units.Count;

    //    return new SteeringTarget {TargetPosition = averagePos, TargetOrientation = Quaternion.AngleAxis(averageRotation, Vector3.up)};
    //}
}
