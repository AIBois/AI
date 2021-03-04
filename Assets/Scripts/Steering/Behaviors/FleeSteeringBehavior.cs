using System.Collections;
using UnityEngine;

public class FleeSteeringBehavior : SteeringBehavior
{

    public override SteeringState? GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!Target || !agent)
            return null;

        //Get direction
        state.linear =  agent.Position - Target.position;
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;

        return state;
    }
}