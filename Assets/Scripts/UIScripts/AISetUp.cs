using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AISetUp : MonoBehaviour
{
    public SquadOption[] options;
    public LearningCumulativeData learningCumulativeData;


    public bool chooseOption(SquadBase playerSquad)
    {
        LearningSquadData playerSquadData = learningCumulativeData.squadsData[(int)playerSquad.squadType];
        float killsOverDeaths = -10000.0f;
        int choosen = Random.Range(0, 3);
        SquadTypes previousSquadType = SquadTypes.NUMSQUADTYPES;
        bool largeSquad = false;
        for (int i = 0; i < options.Length; i++)
        {
            if (previousSquadType == options[i].squadData.squadType && !largeSquad)
            {
                LearningSquadData optionSquadData = learningCumulativeData.squadsData[(int)options[i].squadData.squadType];
                if (optionSquadData.numSquads != 0.0f && playerSquadData.numSquads != 0.0f)
                {
                    float kills = optionSquadData.numOfSquadTypeKilled[(int)playerSquad.squadType] / playerSquadData.numUnits;
                    float deaths = optionSquadData.numKilledBySquadType[(int)playerSquad.squadType] / optionSquadData.numUnits;
                    if (kills - deaths > killsOverDeaths)
                    {
                        previousSquadType = options[i].squadData.squadType;
                        largeSquad = options[i].squadData.largeSquad;
                        killsOverDeaths = kills - deaths;
                        choosen = i;
                    }
                }
            }
        }
        return placeSquad(choosen, playerSquad.transform);
    }

    bool placeSquad(int choosen, Transform playerSquad)
    {
        Vector3 position = playerSquad.position;
        position.z *= -1;
        SquadBase selectedSquad = Instantiate(options[choosen].GetComponent<SquadOption>().squadData.squad.GetComponent<SquadBase>(), position, Quaternion.identity);
        selectedSquad.enemy = true;
        selectedSquad.learningData.numUnits = selectedSquad.Units.Count;
        selectedSquad.learningData.numSquads++;
        return selectedSquad.Large; 
    }

}
