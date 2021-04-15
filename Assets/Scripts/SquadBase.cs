using System;
using System.Collections.Generic;
using States.Squad;
using UnityEngine;

public class SquadBase : MonoBehaviour
{
    public SquadState currentState;
    public IAttackListener attackListener;
    
    public CharacterBase Leader;
    public List<CharacterBase> Units;
    [SerializeField]
    private float safeDistance;

    [SerializeField]
    public bool enemy;

    public float SafeDistance => safeDistance;

    private void Awake()
    {
        SetupSquadSteeringAgents();
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

    void Start()
    {
        currentState = new IdleSquadState(this);
    }

    private void Update()
    {
        currentState.Act();
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
        foreach (var characterBase in Units)
        {
            characterBase.SteeringAgent?.UnitFlee();
            characterBase.SteeringAgent?.SetMovementType(SteeringMovementType.UNIT);
        }
    }

    public void IsBeingAttacked(SquadBase attacker)
    {
        attackListener?.BeingAttacked(attacker);
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