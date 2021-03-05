using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    private float attack, attackDistance, attackTimer, defense;
    [SerializeField]
    private float baseSpeed, retreatSpeed, fov;
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

    public float FOV
    {
        get => fov;
        set => fov = value;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

}
