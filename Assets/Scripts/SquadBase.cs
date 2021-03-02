using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBase : MonoBehaviour
{
    public CharacterBase Leader;
    public CharacterBase[] Units;
    [SerializeField]
    private float cost;

    private void Awake()
    {
        //TODO:: create a randomisation of the cost based ont he individual units.
    }
}
