using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISteering
{
    SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents);
}

public abstract class SteeringBehavior : ISteering
{
    protected const float timeToTarget = 0.1f;

    public abstract SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents = null);

    public static void ClampLinearAcceleration(ref SteeringState state, SteeringAgent agent)
    {
        if (state.linear.sqrMagnitude > agent.MaxAcceleration * agent.MaxAcceleration)
        {
            state.linear.Normalize();
            state.linear *= agent.MaxAcceleration;
        }
    }

    public static void ClampAngularAcceleration(ref SteeringState state, SteeringAgent agent)
    {
        var angularAccleration = Mathf.Abs(state.angular);
        if (angularAccleration > agent.MaxAngularAcceleration)
        {
            state.angular /= angularAccleration;
            state.angular *= agent.MaxAngularAcceleration;
        }
    }

}
