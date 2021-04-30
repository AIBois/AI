using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AISetUp : MonoBehaviour
{
    public SquadBase[] squadBases;
    public SquadOption[] options;
    public LearningCumulativeData learningCumulativeData;


    public void chooseOption(SquadBase playerSquad)
    {
        LearningSquadData playerSquadData = learningCumulativeData.squadsData[(int)playerSquad.squadType];
        float killsOverDeaths = -10000.0f;
        int choosen = Random.Range(0, 3);
        for (int i = 0; i < options.Length; i++)
        {
            LearningSquadData optionSquadData = learningCumulativeData.squadsData[(int)options[i].squadType];
            if (optionSquadData.numSquads != 0.0f && playerSquadData.numSquads != 0.0f)
            {
                float kills = optionSquadData.numOfSquadTypeKilled[(int)playerSquad.squadType] / playerSquadData.numUnits;
                float deaths = optionSquadData.numKilledBySquadType[(int)playerSquad.squadType] / optionSquadData.numUnits;
                if ( kills - deaths > killsOverDeaths )
                {
                    killsOverDeaths = kills - deaths;
                    choosen = i;
                }
            }
        }
        placeSquad(choosen, playerSquad.transform);
    }

    void placeSquad(int choosen, Transform playerSquad)
    {
        Vector3 position = playerSquad.position;
        position.z *= -1;
        SquadBase selectedSquad = Instantiate(squadBases[(int)options[choosen].GetComponent<SquadOption>().squadType], position, Quaternion.identity);
        selectedSquad.enemy = true;
        selectedSquad.learningData.numUnits = selectedSquad.Units.Count;
        selectedSquad.learningData.numSquads++;
    }

}
