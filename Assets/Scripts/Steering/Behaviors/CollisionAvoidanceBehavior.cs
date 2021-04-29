using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceBehavior : SteeringBehavior
{
    private const float RayLength = 2.0f;
    private const float avoidanceLength = 100.0f;

    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents = null)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return state;

        Vector3 vel = agent.Velocity * Time.deltaTime;
        Vector3 dir = vel.normalized;

        RaycastHit hit;
        Ray ray = new Ray(agent.transform.position, dir);
        if (Physics.Raycast(ray, out hit, agent.Velocity.magnitude * RayLength, agent.CollisionMask))
        {
            if (hit.transform.tag == "Unit")
                return state;
            Vector3 leftFrontVector = Quaternion.Euler(0, -65, 0) * dir;
            Vector3 rightFrontVector = Quaternion.Euler(0, 65, 0) * dir;

            RaycastHit leftFrontHit;
            RaycastHit rightFrontHit;
            bool leftFrontRaycast = Physics.Raycast(agent.transform.position, leftFrontVector, out leftFrontHit, RayLength * 2.0f);
            bool rightFrontRaycast = Physics.Raycast(agent.transform.position, rightFrontVector, out rightFrontHit, RayLength * 2.0f);

            Vector3 newTarget;
            if (leftFrontRaycast)
                newTarget = Quaternion.Euler(0, 65, 0) * agent.Velocity;
            else if (rightFrontRaycast)
                newTarget = Quaternion.Euler(0, -65, 0) * agent.Velocity;
            else
            {
                newTarget = -agent.Velocity;
            }

            state.linear = newTarget * avoidanceLength;
        }

        state.linear *= agent.MaxAcceleration * 2.0f;
        ClampLinearAcceleration(ref state, agent);

        return state;
    }
}