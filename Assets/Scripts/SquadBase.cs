using System.Collections;
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

        SetupSquadSteeringAgents();
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
