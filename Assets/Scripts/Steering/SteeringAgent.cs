using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    public float MaxAcceleration = 5.0f;
    public float MaxSpeed = 1.0f;
    public float SlowRadius = 7.5f;
    public float StopRadius = 1.5f;

    public SteeringBehaviorType SteeringType = SteeringBehaviorType.SEEK;

    public Transform target; //This is just for testing


    public Vector3 Position { get; private set; }
    public Vector3 Velocity { get; private set; }
    public Quaternion Orientation { get; private set; }
    public float rotation { get; private set; }

    private SteeringBehavior currentSteeringBehavior;

    void Awake()
    {
        currentSteeringBehavior = SteeringBehaviorFactory.Create(SteeringType);
        Position = transform.position;
    }

    public void IntegrateSteering(SteeringState steering)
    {


        Velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        Debug.Log(Velocity);

        if (Velocity.sqrMagnitude > MaxSpeed * MaxSpeed)
            Velocity = Velocity.normalized * MaxSpeed;

        Position += Velocity * Time.deltaTime;
        transform.position = Position;
    }

    public void Update()
    {
        if(currentSteeringBehavior == null)
            return;

        currentSteeringBehavior.TargetPosition = target.position;
        var steeringInfo = currentSteeringBehavior.GetSteering(this);

        IntegrateSteering(steeringInfo);
    }
}