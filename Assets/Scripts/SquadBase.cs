﻿using System;
using System.Collections.Generic;
using States.Character;
using States.Squad;
using UnityEngine;

public class SquadBase : MonoBehaviour
{
    public SquadState currentState;
    public IAttackListener attackListener;
    
    public CharacterBase Leader;
    public List<CharacterBase> Units;
    [SerializeField]
    private float cost, safeDistance;

    public float SafeDistance => safeDistance;

    private void Awake()
    {
        //TODO:: create a randomisation of the cost based ont he individual units.

        SetupSquadSteeringAgents();
    }

    
    void Start()
    {
        //Test squad move
        MoveTo(new Vector3(-6,0,25));
    }

    public void SetUnitStates(CharacterState state)
    {
        state.context = Leader;
        Leader.currentState = state;
        foreach (var unit in Units)
        {
            state.context = unit;
            unit.currentState = state;
        }
    }

    public void MoveTo(Vector3 position)
    {
        foreach (var characterBase in Units)
        {
            characterBase.SteeringAgent?.SetTarget(position);
            characterBase.SteeringAgent?.SetMovementType(SteeringMovementType.SQUAD);
        }
    }

    public void MoveAwayFrom(Vector3 position)
    {
        throw new NotImplementedException();
    }

    public void IsBeingAttacked(SquadBase attacker)
    {
        attackListener.BeingAttacked(attacker);
    }

    void SetupSquadSteeringAgents()
    {
        foreach (var unit in Units)
        {
            SteeringAgent agent = unit.GetComponent<SteeringAgent>();
            if (!agent) continue;

            agent.Squad = this;
        }
    }

    public Vector3 GetAveragedPosition()
    {
        Vector3 averagePos = Vector3.zero;

        foreach (var unit in Units)
        {
            averagePos += unit.transform.position;

        }
        averagePos /= Units.Count;
        return averagePos;
    }
}