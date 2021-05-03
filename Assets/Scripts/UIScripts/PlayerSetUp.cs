using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetUp: MonoBehaviour
{
    public List<SquadData> squadDatas;

    public List<SquadOption> playerOptions;
    public List<SquadOption> AIOptions;
    public AISetUp AI;
    public SquadPicker picker;
    public int numSquads;
    public int numPlayerLargeSquads;
    public int numAILargeSquads;
    public Text numSquadPicksLeft;
    private bool battleReady = false;
    public Text AISquadTitle, AISquadDesc, PlayerSquadTitle, PlayerSquadDesc;

    private void Awake()
    {
        SetUpOptions();
        SetWinTracking();
        SetUI();
    }

    private void OnEnable()
    {
        battleReady = false;
        SetUpOptions();
        SetWinTracking();
        SetUI();
    }

    private void SetWinTracking()
    {
        WinTracker wins = Object.FindObjectOfType<WinTracker>();
        numSquads = Random.Range(3, 5);
        numPlayerLargeSquads = numSquads == 5 ? 2 : 1;
        numAILargeSquads = numSquads == 5 ? 2 : 1;
        numSquadPicksLeft.text = numSquads.ToString();
        wins.numEnemy = numSquads;
        wins.numPlayer = numSquads;
    }

    private void SetUI()
    {
        AISquadTitle.text = AIOptions[0].gameObject.GetComponent<SquadOption>().squadData.Title;
        AISquadDesc.text = AIOptions[0].gameObject.GetComponent<SquadOption>().squadData.description;
        PlayerSquadTitle.text = playerOptions[0].gameObject.GetComponent<SquadOption>().squadData.Title; ; 
        PlayerSquadDesc.text = playerOptions[0].gameObject.GetComponent<SquadOption>().squadData.description; ;
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
            if(AI.chooseOption(picker.selectedSquad)) numAILargeSquads--;
            if (picker.selectedSquad.Large) numPlayerLargeSquads--;
            picker.selectedSquad = null;
            if (numSquads == 0) battleReady = true;
            else
            {
                SetUpOptions();
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

    void SetUpOptions()
    {
        List<int> alreadySelected = new List<int>();
        for (int i = 0; i < playerOptions.Count; i++)
        {
            if(numPlayerLargeSquads == 0)
            {
                for (int j = squadDatas.Count / 2; j < squadDatas.Count; j++)
                {
                    alreadySelected.Add(j);
                }
            }
            int choosen = Random.Range(0, (int)(squadDatas.Count));
            while(alreadySelected.Contains(choosen))
            {
                choosen = Random.Range(0, (int)(squadDatas.Count));
            }
            alreadySelected.Add(choosen);
            playerOptions[i].OptionSetUp(squadDatas[choosen]);
        }
        alreadySelected.Clear();
        for (int i = 0; i < AIOptions.Count; i++)
        {
            if (numAILargeSquads == 0)
            {
                for (int j = squadDatas.Count / 2; j < squadDatas.Count; j++)
                {
                    alreadySelected.Add(j);
                }
            }
            int choosen = Random.Range(0, (int)(squadDatas.Count));
            while (alreadySelected.Contains(choosen))
            {
                choosen = Random.Range(0, (int)(squadDatas.Count));
            }
            alreadySelected.Add(choosen);
            AIOptions[i].OptionSetUp(squadDatas[choosen]);
        }
    }
}
