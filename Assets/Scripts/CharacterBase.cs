using States.Character;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public CharacterState currentState;
    
    [SerializeField]
    private float attack, attackDistance, attackTimer, defense;
    [SerializeField]
    private float baseSpeed, retreatSpeed;
    [SerializeField]
    private float currentHealth, maxHealth;

    public float Attack
    {
        get => attack;
        set => attack = value;
    }

    public float AttackDistance
    {
        get => attackDistance;
        set => attackDistance = value;
    }

    public float AttackTimer
    {
        get => attackTimer;
        set => attackTimer = value;
    }

    public float Defense
    {
        get => defense;
        set => defense = value;
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

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void MoveTo(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0) currentState = new DeathCharacterState();
    }
}