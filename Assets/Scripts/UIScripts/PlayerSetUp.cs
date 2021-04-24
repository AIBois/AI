using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetUp: MonoBehaviour
{
    public SquadSetUp playerSetUp;
    public SquadSetUp AISetUp;
    public AISetUp AI;
    public SquadPicker picker;
    public Text points;
    private int pointValue;
    private bool battleReady = false;

    private void Awake()
    {
        pointValue = 2000;
    }

    private void Update()
    {
        if (battleReady)
        {
            StartGame();
        }
    }

    public void MouseDown()
    {
        if (!battleReady)
        {
            pointValue -= picker.selectedSquad.Cost;
            points.text = pointValue.ToString();
            AI.chooseOption(picker.selectedSquad.transform);
            picker.selectedSquad = null;
            if (pointValue < 300) battleReady = true;
            else
            {
                playerSetUp.SetUpOptions();
                AISetUp.SetUpOptions();
            }

        }
    }

    public void StartGame()
    {
        SquadBase[] squads = FindObjectsOfType<SquadBase>();
        for(int i = 0; i< squads.Length; i++)
        {
            GameObject squad = squads[i].gameObject;
            squad.GetComponent<SquadBase>().BattleStarted = true;
            for (int j = 0; j < squad.transform.childCount; j++)
            {
                squad.transform.GetChild(j).GetComponent<SteeringAgent>().setPosition();
                squad.transform.GetChild(j).GetComponent<SteeringAgent>().SetMovementType(SteeringMovementType.SQUAD);
            }
        }

        this.transform.parent.gameObject.SetActive(false);        
    }
}
