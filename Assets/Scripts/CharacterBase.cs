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

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0) currentState = new DeathCharacterState(this);
    }

    public bool ReadyToAttack()
    {
        return Time.deltaTime % MeleeAttackFrequency == 0;
    }

    public bool ReadyToRangedAttack()
    {
        return Time.deltaTime % RangedAttackFrequency == 0;
    }
}