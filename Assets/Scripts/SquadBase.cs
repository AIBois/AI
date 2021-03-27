using System;
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
        throw new NotImplementedException();
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
            //Set all unit targets to the leader, this means the squad will flock around the leader
            if (unit != Leader)
                agent.Target = Leader.transform;
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