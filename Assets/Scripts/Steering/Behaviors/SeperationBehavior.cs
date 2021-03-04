using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SeperationBehavior : SteeringBehavior
{
    private const float DistanceThreshold = 20.0f;
    private const float DecayCoefficant = 5.0f;

    public override SteeringState? GetSteering(SteeringAgent agent)
    {
        SteeringState state = new SteeringState();
        if (!Target.HasValue || !agent)
            return null;

        var targets = agent.Squad.Units;

        foreach (var target in targets)
        {
            if(target == agent)
                continue;

            Vector3 direction = agent.Position - target.transform.position;
            float distance2 = direction.sqrMagnitude;

            if (distance2 < DistanceThreshold)
            {
                float strength = Mathf.Min(DecayCoefficant / distance2, agent.MaxAcceleration);
                state.linear += strength * direction.normalized;
            }
        }

        return state;
    }
}