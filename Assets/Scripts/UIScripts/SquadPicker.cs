using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SquadTypes
{
    INFANTRY,
    RANGERS,
    KNIGHTS,
    CAVALRY,
    GIANT,
    NUMSQUADTYPES
}

public class SquadPicker : MonoBehaviour
{
    public SquadBase[] squadBases;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            OptionSetUp(transform.GetChild(i).gameObject);
        }
    }

    void OptionSetUp(GameObject option)
    {
        SquadTypes choosen = (SquadTypes)Random.Range(0, (int)(SquadTypes.NUMSQUADTYPES) - 1);
        GameObject Type = option.transform.GetChild(0).gameObject;
        GameObject Image = option.transform.GetChild(1).gameObject;
        GameObject Cost = option.transform.GetChild(2).gameObject;

        Type.GetComponent<Text>().text = choosen.ToString();
        Image.GetComponent<Image>().color = GetColour(choosen);
        Cost.GetComponent<Text>().text = Random.Range(400, 700).ToString();
    }

    Color GetColour(SquadTypes Type)
    {
        switch(Type)
        {
            case SquadTypes.INFANTRY: return new Color32(204, 70, 70, 255);
            case SquadTypes.RANGERS: return new Color32(12, 154, 59, 255);
            case SquadTypes.KNIGHTS: return new Color32(141, 20, 250, 255);
            case SquadTypes.CAVALRY: return new Color32(28, 113, 236, 255);
            case SquadTypes.GIANT: return new Color32(224, 99, 28, 255);
            default: return new Color32(255, 20, 147, 255);
        }
    }
}
