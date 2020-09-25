using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask boardLayer;
    public LayerMask unitLayer;
    
    private Ray ray;
    private RaycastHit hitData;
    private GameObject selectedTile;

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hitData, 1000, boardLayer))
        {
            GameObject newObject = hitData.transform.gameObject;
            
            if (newObject != selectedTile)
            {
                if (selectedTile != null) selectedTile.GetComponent<Tile>().ToggleSelected();
                
                selectedTile = newObject;
                selectedTile.GetComponent<Tile>().ToggleSelected();
            }
        }
        else
        {
            if (selectedTile != null)
            {
                selectedTile.GetComponent<Tile>().ToggleSelected();
                selectedTile = null;
            }
        }
    }
}
