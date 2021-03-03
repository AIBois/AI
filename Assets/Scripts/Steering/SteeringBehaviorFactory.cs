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
    ALIGN
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
                return new AlighBehavior();
            default:
                throw new ArgumentOutOfRangeException(nameof(steeringType), steeringType, null);
        }
    }
}
