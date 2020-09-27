using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public Image unitThumbnail;
    public string unitName;
    public int unitCost;

    public PlayerController _playerController;

    private void OnMouseDown()
    {

        _playerController.selectedUnit = this.gameObject;
    }

    private void OnMouseDrag()
    {
        _playerController.isDragging = true;
        float x = Mathf.Lerp(transform.position.x, _playerController.selectedTile.transform.position.x, 0.075f);
        float z = Mathf.Lerp(transform.position.z, _playerController.selectedTile.transform.position.z, 0.075f);
        float y = _playerController.selectedTile.transform.position.y + 1f;
        transform.position = new Vector3(x, y, z) ;
    }

    private void OnMouseUp()
    {
        if (_playerController.isDragging) 
            _playerController.isDragging = false;
    }
}
