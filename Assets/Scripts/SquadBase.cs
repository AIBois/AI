using System;
using States.Squad;
using UnityEngine;

public class SquadBase : MonoBehaviour
{
    public SquadState currentState;
    public IAttackListener attackListener;
    
    public CharacterBase Leader;
    public CharacterBase[] Units;
    [SerializeField]
    private float cost;

    private void Awake()
    {
        //TODO:: create a randomisation of the cost based ont he individual units.
    }

    public void SetUnitStates(Type characterState)
    {
        throw new NotImplementedException();
    }

    public void MoveTo(Vector3 position)
    {
        //Move to position
    }

    public void MoveAwayFrom(Vector3 position)
    {
        throw new NotImplementedException();
    }

    public void IsBeingAttacked()
    {
        attackListener.BeingAttacked();
    }
}