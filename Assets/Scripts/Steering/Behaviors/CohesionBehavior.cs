using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CohesionBehavior : SteeringBehavior
{
    public override SteeringState? GetSteering(SteeringAgent agent, Vector3 targetPosition, float targetRotation,
        Vector3 targetVelocity)
    {
        SteeringState state = new SteeringState();
        if (!agent || !agent.Squad)
            return null;

        //Get direction
        state.linear = agent.Squad.GetAveragedPosition() - agent.Position;
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;

        return state;
    }
}