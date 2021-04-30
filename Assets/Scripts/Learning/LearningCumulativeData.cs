using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningCumulativeData : MonoBehaviour
{
    public List<LearningSquadData> squadsData;

    public void AddSquadData(SquadTypes squadType, LearningSquadData squadData)
    {
        squadsData[(int)squadType].Add(squadData);
    }
}
