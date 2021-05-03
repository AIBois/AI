using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadPicker : MonoBehaviour
{
    public SquadBase selectedSquad;
    public new Collider collider;

    private void Update()
    {
        if(selectedSquad)
        {
            Ray mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitData;
            if (collider.Raycast(mousePosition, out hitData, 1000))
            {
                selectedSquad.transform.position = new Vector3(hitData.point.x, 0.0f,hitData.point.z);
            }
        }
    }

    public void SetPrefab(SquadOption option)
    {
        if (selectedSquad) Destroy(selectedSquad.gameObject);
        selectedSquad = Instantiate(option.squadData.squad.GetComponent<SquadBase>(), new Vector3(100.0f, 100.0f, 100.0f), Quaternion.identity);
        selectedSquad.transform.rotation *= Quaternion.Euler(0, 180f, 0);
        selectedSquad.learningData.numUnits = selectedSquad.Units.Count;
        selectedSquad.learningData.numSquads++;        
    }

    
}
