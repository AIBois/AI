using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ArriveBehavior : SteeringBehavior
{
    public override SteeringState? GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!Target.HasValue || !agent)
            return null;

        float targetDistance = Vector3.Distance(agent.Position, Target.Value.TargetPosition) - agent.StopRadius;
        float targetSpeed = agent.MaxSpeed;
        float timeToStop = 0.1f;

        if (targetDistance < agent.SlowRadius) //if within slow radius adjust target speed
        {
            targetSpeed = agent.MaxSpeed * targetDistance / agent.SlowRadius;
        }

        var TargetVelocity = (Target.Value.TargetPosition - agent.Position).normalized;
        TargetVelocity *= targetSpeed;

        state.linear = TargetVelocity - agent.Velocity;
        state.linear /= timeToStop;

        if (state.linear.sqrMagnitude > agent.MaxAcceleration * agent.MaxAcceleration)
        {
            state.linear.Normalize();
            state.linear *= agent.MaxAcceleration;
        }

        state.angular = 0.0f;
        return state;
    }
}