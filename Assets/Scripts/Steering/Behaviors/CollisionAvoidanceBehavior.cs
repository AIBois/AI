using System.Collections;
using UnityEngine;

public class CollisionAvoidanceBehavior : SteeringBehavior
{
    private const float RayLength = 2.0f;
    private const float avoidanceLength = 10.0f;

    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return state;

        //front ray
        RaycastHit hit;
        //if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit, RayLength))
        //{
        //    if (hit.transform.tag != "Unit")
        //    {
        //        Vector3 newTarget = hit.point + (hit.normal * avoidanceLength);
        //        state.linear = newTarget - agent.Position;
        //        ClampLinearAcceleration(ref state, agent);
        //    }
        //}

        Vector3 leftVector = Quaternion.Euler(0, -45, 0) * agent.transform.forward;
        Vector3 rightVector = Quaternion.Euler(0, 45, 0) * agent.transform.forward;

        if (Physics.Raycast(agent.transform.position, leftVector, out hit, RayLength))
        {
            if (hit.transform.tag != "Unit")
            {
                Vector3 newTarget = hit.point + (Vector3.Scale(hit.normal,leftVector) * avoidanceLength);
                state.linear = newTarget - agent.Position;
                state.linear *= agent.MaxAcceleration;
                ClampLinearAcceleration(ref state, agent);
            }
        }

        if (Physics.Raycast(agent.transform.position, rightVector, out hit, RayLength))
        {
            if (hit.transform.tag != "Unit")
            {
                Vector3 newTarget = hit.point + (Vector3.Scale(hit.normal, rightVector) * avoidanceLength);
                state.linear = newTarget - agent.Position;
                state.linear *= agent.MaxAcceleration;
                ClampLinearAcceleration(ref state, agent);
            }
        }




        return state;
    }
}