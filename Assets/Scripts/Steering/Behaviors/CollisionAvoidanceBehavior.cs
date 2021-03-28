using System.Collections;
using UnityEngine;

public class CollisionAvoidanceBehavior : SteeringBehavior
{
    private const float RayLenghth = 2.0f;
    private const float avoidanceLength = 1.0f;

    public override SteeringState? GetSteering(SteeringAgent agent, SteeringTarget target)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return null;

        //front ray
        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit, RayLenghth))
        {
            if (hit.transform.tag != "Unit")
            {
                Vector3 newTarget = hit.point + (hit.normal * avoidanceLength);
                //Get direction
                state.linear = newTarget - agent.Position;
                ClampLinearAcceleration(ref state, agent);
            }
        }


        return state;
    }
}