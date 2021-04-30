using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AISetUp : MonoBehaviour
{
    public SquadBase[] squadBases;
    public SquadOption[] options;
    public LearningCumulativeData learningCumulativeData;


    public void chooseOption(Transform playerSquad)
    {
        int choosen = Random.Range(0, 3);        
        placeSquad(choosen, playerSquad);
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
