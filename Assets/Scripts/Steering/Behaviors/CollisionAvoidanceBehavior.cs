using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceBehavior : SteeringBehavior
{
    private const float RayLength = 2.0f;
    private const float avoidanceLength = 10.0f;

    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents = null)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return state;

        //front ray
        //if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit, RayLength))
        //{
        //    if (hit.transform.tag != "Unit")
        //    {
        //        Vector3 newTarget = hit.point + (hit.normal * avoidanceLength);
        //        state.linear = newTarget - agent.Position;
        //        ClampLinearAcceleration(ref state, agent);
        //    }
        //}

        Vector3 leftFrontVector = Quaternion.Euler(0, -70, 0) * agent.transform.forward;
        Vector3 rightFrontVector = Quaternion.Euler(0, 70, 0) * agent.transform.forward;
        Vector3 rightVector = agent.transform.right;
        Vector3 backVector = Quaternion.Euler(0, 180, 0) * agent.transform.forward;

        RaycastHit leftFrontHit;
        RaycastHit rightFrontHit;
        RaycastHit leftHit;
        RaycastHit rightHit;
        RaycastHit backHit;
        RaycastHit frontHit;

        bool leftFrontRaycast = Physics.Raycast(agent.transform.position, leftFrontVector, out leftFrontHit, RayLength);
        bool rightFrontRaycast = Physics.Raycast(agent.transform.position, rightFrontVector, out rightFrontHit, RayLength);
        bool backRaycast = Physics.Raycast(agent.transform.position, backVector, out backHit, RayLength);
        bool frontRaycast = Physics.Raycast(agent.transform.position, -backVector, out frontHit, RayLength);
        bool rightRaycast = Physics.Raycast(agent.transform.position, rightVector, out rightHit, RayLength * 0.5f);
        bool leftRaycast = Physics.Raycast(agent.transform.position, -rightVector, out leftHit, RayLength * 0.5f);


        if (leftFrontRaycast)
        {
            if (leftFrontHit.transform.tag != "Unit")
            {
                Vector3 newTarget = rightFrontVector * avoidanceLength;
                state.linear += newTarget - agent.Position;
            }
        }

        if (rightFrontRaycast)
        {
            if (rightFrontHit.transform.tag != "Unit")
            {
                Vector3 newTarget = leftFrontVector * avoidanceLength;
                state.linear += newTarget - agent.Position;
            }
        }

        if (rightRaycast)
        {
            if (rightHit.transform.tag != "Unit")
            {
                Vector3 newTarget = -rightVector * avoidanceLength;
                state.linear += newTarget - agent.Position;
            }
        }

        if (leftRaycast)
        {
            if (leftHit.transform.tag != "Unit")
            {
                Vector3 newTarget = rightVector * avoidanceLength;
                state.linear += newTarget - agent.Position;
            }
        }

        if (frontRaycast && 
            ((leftFrontRaycast && !rightFrontRaycast) || (!leftFrontRaycast && rightFrontRaycast)))
        {
            if (frontHit.transform.tag != "Unit")
            {
                Vector3 newTarget = -agent.transform.forward * avoidanceLength * 5.0f;
                state.linear = newTarget - agent.Position;
            }
        }

        if (backRaycast)
        {
            if (backHit.transform.tag != "Unit")
            {
                Vector3 newTarget = agent.transform.forward * avoidanceLength;
                state.linear = newTarget - agent.Position;
            }
        }


        state.linear *= agent.MaxAcceleration;
        ClampLinearAcceleration(ref state, agent);

        return state;
    }
}