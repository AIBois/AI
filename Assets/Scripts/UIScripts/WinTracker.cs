using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTracker : MonoBehaviour
{
    public int AIWins;
    public Text AIWinsText;
    public int PlayerWins;
    public Text PlayerWinsText;
    public int numEnemy, numPlayer;
    public GameObject SetUpUI;

    private void Awake()
    {
        AIWinsText.text = AIWins.ToString();
        PlayerWinsText.text = PlayerWins.ToString();
    }

    public void Update()
    {
        if (numEnemy == 0 || numPlayer == 0)
        {
            UpdateScores();
            SetUpNewGame();
        }
    }

    public void UpdateScores()
    {
        if (numPlayer == 0)
        {
            AIWins++;
            AIWinsText.text = AIWins.ToString();
            numPlayer = -1;
        }
        else if (numEnemy == 0)
        {
            PlayerWins++;
            PlayerWinsText.text = PlayerWins.ToString();
            numEnemy = -1;
        }
    }

    public void removeSquad(SquadBase squad)
    {
        if (squad.enemy) numEnemy--;
        else numPlayer--;
    }

    public void SetUpNewGame()
    {
        SquadBase[] squads = FindObjectsOfType<SquadBase>();
        LearningCumulativeData cumulativeData = FindObjectOfType<LearningCumulativeData>();
        for(int i = 0; i < squads.Length; i++)
        {
            cumulativeData.AddSquadData(squads[i].squadType, squads[i].learningData);
            Destroy(squads[i].gameObject);
        }
        numEnemy = -1;
        numPlayer = -1;
        SetUpUI.SetActive(true);
    }
}
