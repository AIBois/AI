using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadOption : MonoBehaviour
{
    public SquadData squadData;
    public Text squadTypeName;
    public Image squadColour;

    public void OptionSetUp(SquadData squad)
    {
        squadData = squad;
        squadColour.color = squadData.colour;
        squadTypeName.text = squadData.Title;
    }

    public void SetSquadInfo(GameObject infoWindow)
    {
        infoWindow.gameObject.transform.GetChild(0).GetComponent<Text>().text = squadData.Title;
        infoWindow.gameObject.transform.GetChild(1).GetComponent<Text>().text = squadData.description;
    }
}
