using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine.AI;

public class PathfindingBehavior : ArriveBehavior
{
    public override SteeringState GetSteering(SteeringAgent agent, SteeringTarget target, IList<SteeringAgent> groupAgents = null)
    {
        SteeringState state = new SteeringState();
        if (!agent)
            return state;

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(agent.Position, target.Position, NavMesh.AllAreas, path))
        {
            bool isvalid = path.status == NavMeshPathStatus.PathComplete;
            if (!isvalid) 
                return state;

            if (path.corners.Length > 2)
            {
                //Get direction
                state.linear = path.corners[1] - agent.Position;
                state.linear *= agent.MaxAcceleration;
                ClampLinearAcceleration(ref state, agent);
            }
            else
            {
                ArriveVelocity(agent,target,ref state);
            }

        }
        return state;
    }
}