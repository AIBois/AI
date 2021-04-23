using System.Diagnostics;
using States.Character;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public CharacterState currentState;
    
    [SerializeField]
    private float meleeDamage, meleeAttackDistance, meleeAttackFrequency, defense;
    [SerializeField]
    private float rangedDamage, rangedAttackShortDistance, rangedAttackLongDistance, rangedAttackFrequency;
    [SerializeField]
    private float baseSpeed, retreatSpeed, fov;
    [SerializeField]
    private float currentHealth, maxHealth;
    [SerializeField] 
    private bool isRanged;
    
    private readonly Stopwatch stopwatch = new Stopwatch();
    public long timeSinceLastAttack;

    public bool IsRanged => isRanged;

    public float MeleeDamage
    {
        get => meleeDamage;
        set => meleeDamage = value;
    }

    public float MeleeAttackDistance
    {
        get => meleeAttackDistance;
        set => meleeAttackDistance = value;
    }

    public float MeleeAttackFrequency
    {
        get => meleeAttackFrequency;
        set => meleeAttackFrequency = value;
    }

    public float Defense
    {
        get => defense;
        set => defense = value;
    }

    public float RangedDamage
    {
        get => rangedDamage;
        set => rangedDamage = value;
    }

    public float RangedAttackShortDistance
    {
        get => rangedAttackShortDistance;
        set => rangedAttackShortDistance = value;
    }

    public float RangedAttackLongDistance
    {
        get => rangedAttackLongDistance;
        set => rangedAttackLongDistance = value;
    }

    public float RangedAttackFrequency
    {
        get => rangedAttackFrequency;
        set => rangedAttackFrequency = value;
    }

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    public float BaseSpeed
    {
        get => baseSpeed;
        set => baseSpeed = value;
    }

    public float RetreatSpeed
    {
        get => retreatSpeed;
        set => retreatSpeed = value;
    }

    public float FOV
    {
        get => fov;
        set => fov = value;
    }

    public SteeringAgent SteeringAgent { get; set; }

    private void Awake()
    {
        currentHealth = maxHealth;
        SteeringAgent = GetComponent<SteeringAgent>();
        stopwatch.Start();
    }

    private void Update()
    {
        currentState.Act();
    }

    public void MoveTo(Vector3 position)
    {
        SteeringAgent.SetMovementType(SteeringMovementType.UNIT);
        SteeringAgent.SetTarget(position);
    }

    private void TakeDamage(float damage, SquadBase enemySquad)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0) currentState = new DeathCharacterState(this, enemySquad);
    }

    public bool InRangedRange(Vector3 enemyPosition)
    {
        if (!IsRanged) return false;
        var distance = Vector3.Distance(enemyPosition, transform.position);
        return distance <= RangedAttackLongDistance;
    }

    public void RangedAttack(CharacterBase closestEnemy, SquadBase enemySquad)
    {
        if (!ReadyToRangedAttack()) return;
        timeSinceLastAttack = stopwatch.ElapsedMilliseconds;
        closestEnemy.TakeDamage(RangedDamage, enemySquad);
    }
    
    private bool ReadyToRangedAttack()
    {
        return stopwatch.ElapsedMilliseconds - timeSinceLastAttack > RangedAttackFrequency * 1000;
    }

    public bool InMeleeRange(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= MeleeAttackDistance;
    }

    public void MeleeAttack(CharacterBase closestEnemy, SquadBase enemySquad)
    {
        if (!ReadyToMeleeAttack()) return;
        timeSinceLastAttack = stopwatch.ElapsedMilliseconds;
        closestEnemy.TakeDamage(MeleeDamage, enemySquad);
    }
        
    private bool ReadyToMeleeAttack()
    {
        return stopwatch.ElapsedMilliseconds - timeSinceLastAttack > MeleeAttackFrequency * 1000;
    }

    //Draw FOV gizoms
    void OnDrawGizmosSelected()
    {
        float totalFOV = FOV;
        float rayRange = 10.0f;
        float halfFOV = totalFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
    }
}