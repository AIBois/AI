using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BehaviorBlend
{
    private Dictionary<SteeringBehavior, float> behaviorBlends;

    public BehaviorBlend()
    {
        behaviorBlends = new Dictionary<SteeringBehavior, float>();
    }

    public void AddBlend(SteeringBehavior behavior, float weight)
    {
        behaviorBlends[behavior] = weight;
    }

    public SteeringState GetSteering(SteeringAgent agent)
    {
        Vector3 linear = Vector3.zero;
        float angular = 0.0f;

        foreach (var behaviorBlend in behaviorBlends.OrderByDescending(x => x.Value))
        {
            var state = behaviorBlend.Key.GetSteering(agent);
            if (state.HasValue)
            {
                linear += state.Value.linear * behaviorBlend.Value;
                angular += state.Value.angular * behaviorBlend.Value;
            }
        }

        if (linear.sqrMagnitude > agent.MaxAcceleration * agent.MaxAcceleration)
        {
            linear.Normalize();
            linear *= agent.MaxAcceleration;
        }

        var angularAccleration = Mathf.Abs(angular);
        if (angularAccleration > agent.MaxAngularAcceleration)
        {
            angular /= angularAccleration;
            angular *= agent.MaxAngularAcceleration;
        }

        return new SteeringState{angular = angular, linear = linear};
    }
}