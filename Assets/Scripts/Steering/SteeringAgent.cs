using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct SteeringBlend
{
    public SteeringBehaviorType type;
    public float weight;
}

public class SteeringAgent : MonoBehaviour
{
    #region Member vars
    [SerializeField] private float maxAcceleration = 10.0f;
    [SerializeField] private float maxAngularAcceleration = 10.0f;
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float maxRotation = 15.0f;
    [SerializeField] private float slowRadius = 7.5f;
    [SerializeField] private float stopRadius = 1.5f;
    [SerializeField] private List<SteeringBlend> steeringBlendTypes;
    [SerializeField] private Transform target;

    private BehaviorBlend behaviorBlend;
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
    public float MaxSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }
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
    public Transform Target
    {
        get => target;
        set => target = value;
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
        behaviorBlend = new BehaviorBlend();
        foreach (var blend in steeringBlendTypes)
        {
            behaviorBlend.AddBlend(SteeringBehaviorFactory.Create(blend.type),blend.weight);
        }

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

        Debug.Log(Velocity);

        if (Velocity.sqrMagnitude > MaxSpeed * MaxSpeed)
            Velocity = Velocity.normalized * MaxSpeed;

        Position += Velocity * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up);
        Debug.Log(transform.rotation);
    }

    public void Update()
    {
        if(behaviorBlend == null)
            return;

        //Set Target;
        //CurrentSteeringBehavior.SetTarget(Target);
        var steeringInfo = behaviorBlend.GetSteering(this);
        IntegrateSteering(steeringInfo);
    }
}