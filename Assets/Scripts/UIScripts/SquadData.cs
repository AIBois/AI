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
    Ogre,
    NUMSQUADTYPES
}

[CreateAssetMenu(menuName = "SquadData")]
public class SquadData : ScriptableObject
{
    public SquadTypes squadType;
    public string Title;
    public string description;
    public Color colour;
    public GameObject squad;
    public bool largeSquad;
}
