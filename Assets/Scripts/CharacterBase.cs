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

    [SerializeField]
    private SquadTypes squadType;

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

    private void MoveTo(Vector3 position, float orientation)
    {
        SteeringAgent.SetMovementType(SteeringMovementType.UNIT);
        SteeringAgent.SetTarget(position,orientation);
    }

    public void MoveTo(CharacterBase character)
    {
        SteeringAgent.SetMovementType(SteeringMovementType.UNIT);
        SteeringAgent.SetTarget(character);
    }

    public void Regroup()
    {
        SteeringAgent.SetMovementType(SteeringMovementType.REGROUP);
    }

    private bool TakeDamage(float damage, SquadBase ownSquad, SquadTypes enemySquadType)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            currentState = new DeathCharacterState(this, ownSquad, enemySquadType);
            return true;
        }
        return false;
    }

    public bool InRangedRange(Vector3 enemyPosition)
    {
        if (!IsRanged) return false;
        var distance = Vector3.Distance(enemyPosition, transform.position);
        return distance <= RangedAttackLongDistance;
    }

    public void RangedAttack(CharacterBase closestEnemy, SquadBase enemySquad)
    {
        if (closestEnemy.currentHealth <= 0.0f) return;
        MoveTo(transform.position, GetRotationToCharacter(closestEnemy));
        if (!ReadyToRangedAttack()) return;
        timeSinceLastAttack = stopwatch.ElapsedMilliseconds;
        SquadTypes enemyType = closestEnemy.squadType;
        if(closestEnemy.TakeDamage(RangedDamage, enemySquad, squadType))
        {
            gameObject.transform.parent.GetComponent<SquadBase>().learningData.numOfSquadTypeKilled[(int)enemyType]++;
        }
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
        if (closestEnemy.currentHealth <= 0.0f) return;
        MoveTo(transform.position, GetRotationToCharacter(closestEnemy));
        if (!ReadyToMeleeAttack()) return;
        timeSinceLastAttack = stopwatch.ElapsedMilliseconds;
        SquadTypes enemyType = closestEnemy.squadType;
        if (closestEnemy.TakeDamage(MeleeDamage, enemySquad, squadType))
        {
            gameObject.transform.parent.GetComponent<SquadBase>().learningData.numOfSquadTypeKilled[(int)enemyType]++;
        }
    }
        
    private bool ReadyToMeleeAttack()
    {
        return stopwatch.ElapsedMilliseconds - timeSinceLastAttack > MeleeAttackFrequency * 1000;
    }

    private float GetRotationToCharacter(CharacterBase character)
    {
        Vector3 direction = character.transform.position - transform.position;
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
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