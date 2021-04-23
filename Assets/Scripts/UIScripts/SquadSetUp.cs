using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SquadTypes
{
    Infantry,
    Rangers,
    Knights,
    Cavalry,
    Giant,
    NUMSQUADTYPES
}

public class SquadSetUp : MonoBehaviour
{
    public NumSquads numSquads;

    private void Start()
    {
        SetUpOptions();
    }

    public void SetUpOptions()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            OptionSetUp(transform.GetChild(i).gameObject);
        }
    }

    void OptionSetUp(GameObject option)
    {
        SquadOption squadOption = option.GetComponent<SquadOption>();

        squadOption.squadType = (SquadTypes)Random.Range(0, (int)(SquadTypes.NUMSQUADTYPES));
        squadOption.squadColour.color = GetColour(squadOption.squadType);
        squadOption.squadTypeName.text = squadOption.squadType.ToString();
        squadOption.cost = GetPoints();
        squadOption.squadCost.text = squadOption.cost.ToString();
    }

    Color GetColour(SquadTypes Type)
    {
        switch (Type)
        {
            case SquadTypes.Infantry: return new Color32(204, 70, 70, 255);
            case SquadTypes.Rangers: return new Color32(12, 154, 59, 255);
            case SquadTypes.Knights: return new Color32(141, 20, 250, 255);
            case SquadTypes.Cavalry: return new Color32(28, 113, 236, 255);
            case SquadTypes.Giant: return new Color32(224, 99, 28, 255);
            default: return new Color32(255, 20, 147, 255);
        }
    }

    int GetPoints()
    {
        switch (numSquads.numSquads)
        {
            case 3:
                return 600 + Random.Range(-50, 50);                
            case 4:
                return 450 + Random.Range(-50, 50);                
            case 5:
                return 350 + Random.Range(-50, 50);
            default:
                return 1000;
        }

    }
}
