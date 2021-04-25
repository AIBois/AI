using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum SteeringBehaviorType
{
    SEEK,
    FLEE,
    ARRIVE,
    ALIGN,
    SEPARATION,
    VELOCITY_MATCH,
    COHESION,
    COLLISION_AVOIDANCE,
    PATHFINDING,
    FACE
}

public class SteeringBehaviorFactory
{
    public static SteeringBehavior Create(SteeringBehaviorType steeringType)
    {
        switch (steeringType)
        {
            case SteeringBehaviorType.SEEK:
                return new SeekSteeringBehavior();
            case SteeringBehaviorType.FLEE:
                return new FleeSteeringBehavior();
            case SteeringBehaviorType.ARRIVE:
                return new ArriveBehavior();
            case SteeringBehaviorType.ALIGN:
                return new AlignBehavior();
            case SteeringBehaviorType.SEPARATION:
                return new SeperationBehavior();
            case SteeringBehaviorType.VELOCITY_MATCH:
                return new VelocityMatchBehavior();
            case SteeringBehaviorType.COHESION:
                return new CohesionBehavior();
            case SteeringBehaviorType.COLLISION_AVOIDANCE:
                return new CollisionAvoidanceBehavior();
            case SteeringBehaviorType.PATHFINDING:
                return new PathfindingBehavior();
            case SteeringBehaviorType.FACE:
                return new FaceSteeringBehavior();
            default:
                throw new ArgumentOutOfRangeException(nameof(steeringType), steeringType, null);
        }
    }
}
