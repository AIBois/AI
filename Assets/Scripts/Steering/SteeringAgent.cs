using System;
using System.Collections.Generic;
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
    UNIT
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
    [SerializeField] private List<SteeringBlend> steeringBlendTypes;
    private CharacterBase characterBase;
    private SteeringTarget steeringTarget;
    private SteeringMovementType movementTypeState;
    private BehaviorBlend SquadBehaviorBlend;
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

        SquadBehaviorBlend = new BehaviorBlend();
        foreach (var blend in steeringBlendTypes)
        {
            SquadBehaviorBlend.AddBlend(SteeringBehaviorFactory.Create(blend.type),blend.weight);
        }

        Position = transform.position;
        rotation = transform.rotation.eulerAngles.y;

        movementTypeState = SteeringMovementType.SQUAD;
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
        transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up);
        Debug.Log(transform.rotation);
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
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetTarget(SteeringTarget target)
    {
        steeringTarget = target;
    }

    public void SetTarget(Vector3 position)
    {
        steeringTarget.Position = position;
    }

    public void SetMovementType(SteeringMovementType movementType)
    {
        movementTypeState = movementType;
    }

    /// <summary>
    /// Squad move uses a flocking based algorithm to move the squad as a whole centered around its leader
    /// </summary>
    /// <param name="target"></param>
    private void SquadMove()
    {
        if (SquadBehaviorBlend == null)
            return;

        var flockSteeringInfo =  SquadBehaviorBlend.GetSteering(this, steeringTarget);

        var arriveTarget = SteeringBehaviorFactory.Create(SteeringBehaviorType.ARRIVE);
        var arriveSteering = arriveTarget.GetSteering(this, steeringTarget);

        //var finalSteering = arriveSteering.linear;
        //finalSteering += flockSteeringInfo.linear * 10.0f;
        //flockSteeringInfo.linear = finalSteering;

        //Final blend 
        BehaviorBlend squadMoveBlend = new BehaviorBlend(BlendType.ADD);
        squadMoveBlend.AddBlend(arriveTarget, 1.0f);
        squadMoveBlend.AddBlend(SquadBehaviorBlend, 5.0f);

        BehaviorBlend finalBehaviorBlend = new BehaviorBlend(BlendType.LERP);
        finalBehaviorBlend.AddBlend(squadMoveBlend, 1.0f);

        //collision avoidance
        var collisionBehavior = SteeringBehaviorFactory.Create(SteeringBehaviorType.COLLISION_AVOIDANCE);
        var collisionSteering = collisionBehavior.GetSteering(this, steeringTarget);
        if (collisionSteering.linear.sqrMagnitude >= 0.1f)
        {
            finalBehaviorBlend.AddBlend(collisionBehavior,0.9f);
        }

        var finalSteering = finalBehaviorBlend.GetSteering(this, steeringTarget);

        SteeringBehavior.ClampLinearAcceleration(ref finalSteering, this);
        
        IntegrateSteering(finalSteering);
    }

    private void UnitMove()
    {
        var arriveTarget = SteeringBehaviorFactory.Create(SteeringBehaviorType.ARRIVE);
        var arriveSteering = arriveTarget.GetSteering(this, steeringTarget);
        IntegrateSteering(arriveSteering);
    }
}