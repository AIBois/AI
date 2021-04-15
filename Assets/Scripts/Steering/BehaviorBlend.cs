using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum BlendType
{
    ADD,
    LERP
}

public class BehaviorBlend : ISteering
{
    private readonly Dictionary<ISteering, float> behaviorBlends;
    private readonly BlendType blendType;

    public BehaviorBlend(BlendType blendType = BlendType.ADD)
    {
        behaviorBlends = new Dictionary<ISteering, float>();
        this.blendType = blendType;
    }

    public void AddBlend(ISteering behavior, float weight)
    {
        behaviorBlends[behavior] = weight;
    }

    private void AdditiveBlend(SteeringAgent agent, SteeringTarget target, ref Vector3 linear, ref float angular, IList<SteeringAgent> groupAgents)
    {
        foreach (var behaviorBlend in behaviorBlends)
        {
            var state = behaviorBlend.Key.GetSteering(agent, target, groupAgents);
            linear += state.linear * behaviorBlend.Value;
            angular += state.angular * behaviorBlend.Value;
        }
    }

    private void LerpBlend(SteeringAgent agent, SteeringTarget target, ref Vector3 linear, ref float angular, IList<SteeringAgent> groupAgents)
    {
        Vector3 lastLinear = Vector3.zero;
        float lastAngular = 0.0f;

        foreach (var behaviorBlend in behaviorBlends)
        {
            var state = behaviorBlend.Key.GetSteering(agent, target, groupAgents);
            Vector3 newLinear = Vector3.LerpUnclamped(lastLinear, state.linear, behaviorBlend.Value);
            float newAngular = Mathf.LerpUnclamped(lastAngular, state.angular, behaviorBlend.Value);
            lastLinear = newLinear;
            lastAngular = newAngular;
        }

        linear = lastLinear;
        angular = lastAngular;
    }

    public SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents = null)
    {
        Vector3 linear = Vector3.zero;
        float angular = 0.0f;

        switch (blendType)
        {
            case BlendType.ADD:
                AdditiveBlend(agent, target, ref linear, ref angular, groupAgents);
                break;
            case BlendType.LERP:
                LerpBlend(agent, target, ref linear, ref angular, groupAgents);
                break;
            default:
                throw new ArgumentOutOfRangeException();
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