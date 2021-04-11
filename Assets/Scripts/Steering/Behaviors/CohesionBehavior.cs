using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CohesionBehavior : SteeringBehavior
{
    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents)
    {
        SteeringState state = new SteeringState();
        if (!agent || !agent.Squad)
            return state;

        //Get direction
        state.linear = GetAveragedPosition(groupAgents) - agent.Position;
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;

        return state;
    }

    private Vector3 GetAveragedPosition(IList<SteeringAgent> groupAgents)
    {
        Vector3 averagePos = Vector3.zero;

        foreach (var unit in groupAgents)
        {
            averagePos += unit.transform.position;

        }
        averagePos /= groupAgents.Count;
        return averagePos;
    }
}