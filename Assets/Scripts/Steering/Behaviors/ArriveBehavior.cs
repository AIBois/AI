﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ArriveBehavior : SteeringBehavior
{
    public override SteeringState? GetSteering(SteeringAgent agent, SteeringTarget target)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return null;

        float targetDistance = Vector3.Distance(agent.Position, target.Position) - agent.StopRadius;
        float targetSpeed = agent.MaxSpeed;

        if (targetDistance < agent.SlowRadius) //if within slow radius adjust target speed
        {
            targetSpeed = agent.MaxSpeed * targetDistance / agent.SlowRadius;
        }

        var TargetVelocity = (target.Position - agent.Position).normalized;
        TargetVelocity *= targetSpeed;

        state.linear = TargetVelocity - agent.Velocity;
        state.linear /= timeToTarget;
        ClampLinearAcceleration(ref state, agent);

        state.angular = 0.0f;
        return state;
    }
}