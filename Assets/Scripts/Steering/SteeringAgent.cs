using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
struct SteeringBlend
{
    public SteeringBehaviorType type;
    public float weight;
}

public enum SteeringMovementType
{
    SQUAD,
    UNIT,
    FLEE,
    REGROUP,
    NONE
}

[RequireComponent(typeof(CharacterBase))]
public class SteeringAgent : MonoBehaviour
{
    #region Member vars
    [SerializeField] private float maxAcceleration = 10.0f;
    [SerializeField] private float maxAngularAcceleration = 10.0f;
    [SerializeField] private float maxRotation = 15.0f;
    [SerializeField] private float slowRadius = 7.5f;
    [SerializeField] private float stopRadius = 1.5f;
    [SerializeField] private float flockViewDistance = 5.0f;
    [SerializeField] private float squadFlockDistanceThreshold = 5.0f;
    [SerializeField] [Range(0.0f,1.0f)] private float squadFlockLerpAmount = 0.9f;
    [SerializeField] private float squadFlockStrength = 100.0f;
    [SerializeField] private List<SteeringBlend> steeringBlendTypes;
    private CharacterBase characterBase;
    private SteeringTarget steeringTarget;
    private SteeringMovementType movementTypeState;
    private BehaviorBlend FlockFOVBehaviorBlend;
    public LayerMask CollisionMask;
    #endregion

    #region properties
    public float MaxAcceleration
    {
        get => maxAcceleration;
        set => maxAcceleration = value;
    }
    public float MaxAngularAcceleration
    {
        get => maxAngularAcceleration;
        set => maxAngularAcceleration = value;
    }

    public float MaxSpeed => characterBase.BaseSpeed;

    public float MaxRotation
    {
        get => maxRotation;
        set => maxRotation = value;
    }
    public float SlowRadius
    {
        get => slowRadius;
        set => slowRadius = value;
    }
    public float StopRadius
    {
        get => stopRadius;
        set => stopRadius = value;
    }

    public SquadBase Squad { get; set; }
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public Vector3 Velocity { get; private set; }
    public Quaternion Orientation
    {
        get => transform.rotation;
        set => transform.rotation = value;
    }
    public float rotation { get; private set; }
    #endregion

    void Awake()
    {
        characterBase = GetComponent<CharacterBase>();

        FlockFOVBehaviorBlend = new BehaviorBlend();
        foreach (var blend in steeringBlendTypes)
        {
            FlockFOVBehaviorBlend.AddBlend(SteeringBehaviorFactory.Create(blend.type),blend.weight);
        }
        
        setPosition();

        movementTypeState = SteeringMovementType.NONE;
    }

    public void setPosition()
    {
        Position = transform.position;
        rotation = transform.rotation.eulerAngles.y;
    }

    public void IntegrateSteering(SteeringState steering)
    {
        var collisionBehavior = SteeringBehaviorFactory.Create(SteeringBehaviorType.COLLISION_AVOIDANCE);
        var collisionSteering = collisionBehavior.GetSteering(this, steeringTarget);
        if (collisionSteering.linear.sqrMagnitude >= 0.01f)
        {
            steering = collisionSteering;
        }

        Velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        //Mask out Y axis
        Velocity = Vector3.Scale(Velocity, new Vector3(1, 0, 1));

        if (Velocity.sqrMagnitude > MaxSpeed * MaxSpeed)
            Velocity = Velocity.normalized * MaxSpeed;

        Position += Velocity * Time.deltaTime;
        Orientation = Quaternion.AngleAxis(rotation, Vector3.up);
    }

    public void Update()
    {
        switch (movementTypeState)
        {
            case SteeringMovementType.SQUAD:
                SquadMove();
                break;
            case SteeringMovementType.UNIT:
                UnitMove();
                break;
            case SteeringMovementType.FLEE:
                UnitFlee();
                break;
            case SteeringMovementType.REGROUP:
                SquadRegroup();
                break;
            case SteeringMovementType.NONE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //public void SetTarget(SteeringTarget target)
    //{
    //    steeringTarget.CharacterBase = null;
    //    steeringTarget = target;
    //}

    public void SetTarget(CharacterBase target)
    {
        steeringTarget.CharacterBase = target;
    }

    public void SetTarget(Vector3 position, float orientation)
    {
        steeringTarget.CharacterBase = null;
        steeringTarget.SetPosition(position);
        steeringTarget.Rotation = orientation;
    }

    public void SetMovementType(SteeringMovementType movementType)
    {
        movementTypeState = movementType;
    }

    private IList<SteeringAgent> GetAgentsWithinFOV()
    {
        List<SteeringAgent> agents = new List<SteeringAgent>();

        var overlappingObjects = Physics.OverlapSphere(transform.position, flockViewDistance);
        foreach (var overlappingObject in overlappingObjects)
        {
            if (overlappingObject.tag == "Unit")
            {
                SteeringAgent agent = overlappingObject.GetComponent<SteeringAgent>();
                if(agent)
                    agents.Add(agent);
            }
        }

        return agents;
    }

    /// <summary>
    /// Squad move uses a flocking based algorithm to move the squad as a whole centered around its leader
    /// </summary>
    /// <param name="target"></param>
    private void SquadMove()
    {
        if (FlockFOVBehaviorBlend == null)
            return;

        var arriveTarget = SteeringBehaviorFactory.Create(SteeringBehaviorType.PATHFINDING);
        var faceTarget = SteeringBehaviorFactory.Create(SteeringBehaviorType.FACE);

        //Final blend 
        BehaviorBlend squadMoveBlend = new BehaviorBlend(BlendType.ADD);
        squadMoveBlend.AddBlend(arriveTarget, 1.0f);
        squadMoveBlend.AddBlend(FlockFOVBehaviorBlend, 5.0f);
        squadMoveBlend.AddBlend(faceTarget, 6.0f);

        SteeringState finalSteering = squadMoveBlend.GetSteering(this, steeringTarget, GetAgentsWithinFOV());

        target = finalSteering.linear;
        ////Squad flocking is to ensure that the squad units don't seperate to far from each other, if a unit in a squad is too far from the center they will slow down
        var squadFlockSteering = FlockFOVBehaviorBlend.GetSteering(this, steeringTarget, Squad.Units.Select(unit => unit.SteeringAgent).ToList());
        if (squadFlockSteering.linear.sqrMagnitude > squadFlockDistanceThreshold)
        {
            finalSteering.linear = Vector3.Lerp(finalSteering.linear, squadFlockSteering.linear, squadFlockLerpAmount)
                                   * squadFlockStrength;
        }

        SteeringBehavior.ClampLinearAcceleration(ref finalSteering, this);
        
        IntegrateSteering(finalSteering);
    }

    private void UnitMove()
    {
        var arriveTarget = SteeringBehaviorFactory.Create(SteeringBehaviorType.PATHFINDING);
        var faceTarget = SteeringBehaviorFactory.Create(SteeringBehaviorType.FACE);

        BehaviorBlend squadMoveBlend = new BehaviorBlend(BlendType.ADD);
        squadMoveBlend.AddBlend(arriveTarget, 1.0f);
        squadMoveBlend.AddBlend(FlockFOVBehaviorBlend, 5.0f);
        squadMoveBlend.AddBlend(faceTarget, 6.0f);

        var arriveSteering = squadMoveBlend.GetSteering(this, steeringTarget, GetAgentsWithinFOV());
        IntegrateSteering(arriveSteering);
    }

    private void SquadRegroup()
    {
        var cohesion = new CohesionBehavior();
        var seperation = new SeperationBehavior();
        var align = new AlignBehavior();

        BehaviorBlend regroupBlend = new BehaviorBlend(BlendType.ADD);
        regroupBlend.AddBlend(cohesion, 5.0f);
        regroupBlend.AddBlend(seperation, 1.0f);
        regroupBlend.AddBlend(align, 1.0f);

        var squadFlockSteering = regroupBlend.GetSteering(this, steeringTarget, Squad.Units.Select(unit => unit.SteeringAgent).ToList());
        squadFlockSteering.linear *= characterBase.RetreatSpeed;
        SteeringBehavior.ClampLinearAcceleration(ref squadFlockSteering, this);
        squadFlockSteering.linear *= 1.25f;

        IntegrateSteering(squadFlockSteering);
    }

    public void UnitFlee()
    {
        var fleeTarget = SteeringBehaviorFactory.Create(SteeringBehaviorType.FLEE);

        BehaviorBlend fleeMoveBlend = new BehaviorBlend(BlendType.ADD);
        fleeMoveBlend.AddBlend(fleeTarget, 1.0f);
        fleeMoveBlend.AddBlend(FlockFOVBehaviorBlend, 0.4f);

        var fleeSteering = fleeMoveBlend.GetSteering(this, steeringTarget, GetAgentsWithinFOV());
        SteeringBehavior.ClampLinearAcceleration(ref fleeSteering, this);
        fleeSteering.linear *= characterBase.RetreatSpeed;

        IntegrateSteering(fleeSteering);
    }

    //Draw FOV gizoms
    private Vector3 target;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Velocity);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, target);
    }
}