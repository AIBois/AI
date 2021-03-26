using System;
using States.Character;
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
        //TODO:: create a randomisation of the cost based on the individual units.
    }

    public void SetUnitStates(CharacterState state)
    {
        state.context = Leader;
        Leader.currentState = state;
        foreach (var unit in Units)
        {
            state.context = unit;
            unit.currentState = state;
        }
    }

    public void MoveTo(Vector3 position)
    {
        throw new NotImplementedException();
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