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

    private void Awake()
    {
        pointValue = 2000;
    }

    public void MouseDown()
    {
        pointValue -= picker.selectedSquad.Cost;
        points.text = pointValue.ToString();
        AI.chooseOption(picker.selectedSquad.transform);
        picker.selectedSquad = null;
        playerSetUp.SetUpOptions();
        AISetUp.SetUpOptions();
    }
}
