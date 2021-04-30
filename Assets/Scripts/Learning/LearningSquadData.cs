using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningSquadData : MonoBehaviour
{
    public int numSquads;
    public int numUnits;
    public List<int> numOfSquadTypeKilled;
    public List<int> numKilledBySquadType;

    private void Awake()
    {
        numSquads = 0;
        numUnits = 0;
        for (int i = 0; i < (int)SquadTypes.NUMSQUADTYPES; i++)
        {
            numOfSquadTypeKilled.Add(0);
            numKilledBySquadType.Add(0);
        }
    }

    public void Add(LearningSquadData otherSquadData)
    {
        numSquads += otherSquadData.numSquads;
        numUnits  += otherSquadData.numUnits;
        for (int i = 0; i < (int)SquadTypes.NUMSQUADTYPES; i++)
        {
            numOfSquadTypeKilled[i] += otherSquadData.numOfSquadTypeKilled[i];
            numKilledBySquadType[i] += otherSquadData.numKilledBySquadType[i];
        }
    }
}
