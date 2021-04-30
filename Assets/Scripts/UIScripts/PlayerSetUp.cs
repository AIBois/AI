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
    public int numSquads;
    public Text numSquadPicksLeft;
    private bool battleReady = false;

    private void Awake()
    {
        WinTracker wins = Object.FindObjectOfType<WinTracker>();
        numSquads = Random.Range(3, 5);
        numSquadPicksLeft.text = numSquads.ToString();
        wins.numEnemy = numSquads;
        wins.numPlayer = numSquads;
    }

    private void OnEnable()
    {
        battleReady = false;
        playerSetUp.SetUpOptions();
        AISetUp.SetUpOptions();
        WinTracker wins = Object.FindObjectOfType<WinTracker>();
        numSquads = Random.Range(3, 5);
        numSquadPicksLeft.text = numSquads.ToString();
        wins.numEnemy = numSquads;
        wins.numPlayer = numSquads;
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
        if (!battleReady && picker.selectedSquad)
        {
            numSquads--;
            numSquadPicksLeft.text = numSquads.ToString();
            AI.chooseOption(picker.selectedSquad);
            picker.selectedSquad = null;
            if (numSquads == 0) battleReady = true;
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
