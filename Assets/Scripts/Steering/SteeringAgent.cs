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
        const float dampening = 0.99f;
        Velocity *= dampening;

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

    public void SetTarget(Vector3 position)
    {
        steeringTarget.CharacterBase = null;
        steeringTarget.SetPosition(position);
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


        BehaviorBlend finalBehaviorBlend = new BehaviorBlend(BlendType.LERP);
        finalBehaviorBlend.AddBlend(squadMoveBlend, 1.0f);


        //collision avoidance
        var collisionBehavior = SteeringBehaviorFactory.Create(SteeringBehaviorType.COLLISION_AVOIDANCE);
        var collisionSteering = collisionBehavior.GetSteering(this, steeringTarget);
        if (collisionSteering.linear.sqrMagnitude >= 0.1f)
        {
            finalBehaviorBlend.AddBlend(collisionBehavior,0.9f);
        }

        var finalSteering = finalBehaviorBlend.GetSteering(this, steeringTarget, GetAgentsWithinFOV());

        //Squad flocking is to ensure that the squad units don't seperate to far from each other, if a unit in a squad is too far from the center they will slow down
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

    public void UnitFlee()
    {
        var target = SteeringBehaviorFactory.Create(SteeringBehaviorType.FLEE);
        var fleeSteering = target.GetSteering(this, steeringTarget);
        IntegrateSteering(fleeSteering);
    }
}