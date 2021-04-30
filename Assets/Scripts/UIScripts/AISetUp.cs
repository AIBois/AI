using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AISetUp : MonoBehaviour
{
    public SquadBase[] squadBases;
    public Text points;
    private int pointValue;
    public SquadOption[] options;

    private void Awake()
    {
        pointValue = 2000;
    }

    private void OnEnable()
    {
        pointValue = 2000;
        points.text = pointValue.ToString();
    }

    public void chooseOption(Transform playerSquad)
    {
        int choosen = 0;
        int cost = 1000;
        for(int i = 0; i < options.Length; i++)
        {
            if(options[i].cost < cost)
            {
                cost = options[i].cost;
                choosen = i;
            }
        }
        placeSquad(choosen, playerSquad);
    }

    void placeSquad(int choosen, Transform playerSquad)
    {
        Vector3 position = playerSquad.position;
        position.z *= -1;
        SquadBase selectedSquad = Instantiate(squadBases[(int)options[choosen].GetComponent<SquadOption>().squadType], position, Quaternion.identity);
        selectedSquad.Cost = options[choosen].GetComponent<SquadOption>().cost;
        selectedSquad.enemy = true;
        pointValue -= selectedSquad.Cost;
        points.text = pointValue.ToString();
        selectedSquad.learningData.numUnits = selectedSquad.Units.Count;
        selectedSquad.learningData.numSquads++;
    }

}
