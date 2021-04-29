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
    private bool battleStarted = false;

    [SerializeField]
    public bool enemy;
    private int cost;

    public float SafeDistance => safeDistance;

    public int Cost
    {
        get => cost;
        set => cost = value;
    }

    public bool BattleStarted
    {
        get => battleStarted;
        set => battleStarted = value;
    }

    private void Awake()
    {
       SetupSquadSteeringAgents();
    }
    
    public void SetupSquadSteeringAgents()
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
        if(battleStarted)
            currentState.Act();
    }

    public void MoveTo(Vector3 position)
    {
        foreach (var characterBase in Units)
        {
            characterBase.SteeringAgent?.SetTarget(position, 0.0f);
            characterBase.SteeringAgent?.SetMovementType(SteeringMovementType.SQUAD);
        }
    }

    public void MoveTo(CharacterBase character)
    {
        foreach (var characterBase in Units)
        {
            characterBase.SteeringAgent?.SetTarget(character);
            characterBase.SteeringAgent?.SetMovementType(SteeringMovementType.SQUAD);
        }
    }

    public void MoveAwayFrom(Vector3 position)
    {
        foreach (var characterBase in Units)
        {
            characterBase.SteeringAgent?.SetTarget(position, 0.0f);
            characterBase.SteeringAgent?.SetMovementType(SteeringMovementType.FLEE);
        }
    }

    public Vector3 GetAveragedPosition()
    {
        Vector3 averagePos = Vector3.zero;

        foreach (var unit in Units)
        {
            averagePos += unit.transform.position;

        }
        return Units.Count > 1 ? averagePos /= Units.Count : averagePos;
    }
}