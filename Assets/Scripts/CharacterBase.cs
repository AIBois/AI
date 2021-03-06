using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    private float meleeAttack, meleeAttackDistance, meleeAttackTimer, defense;
    [SerializeField]
    private float rangedAttack, rangedAttackDistance, rangedAttackTimer;
    [SerializeField]
    private float baseSpeed, retreatSpeed;
    [SerializeField]
    private float currentHealth, maxHealth;

    public float MeleeAttack
    {
        get => meleeAttack;
        set => meleeAttack = value;
    }

    public float MeleeAttackDistance
    {
        get => meleeAttackDistance;
        set => meleeAttackDistance = value;
    }

    public float MeleeAttackTimer
    {
        get => meleeAttackTimer;
        set => meleeAttackTimer = value;
    }

    public float Defense
    {
        get => defense;
        set => defense = value;
    }

    public float RangedAttack
    {
        get => rangedAttack;
        set => rangedAttack = value;
    }

    public float RangedAttackDistance
    {
        get => rangedAttackDistance;
        set => rangedAttackDistance = value;
    }

    public float RangedAttackTimer
    {
        get => rangedAttackTimer;
        set => rangedAttackTimer = value;
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

}
