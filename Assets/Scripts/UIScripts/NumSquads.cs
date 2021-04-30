using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumSquads : MonoBehaviour
{
    public int numSquads;

    private void Awake()
    {
        numSquads = Random.Range(3, 5);
    }

    private void OnEnable()
    {
        numSquads = Random.Range(3, 5);
    }

}
