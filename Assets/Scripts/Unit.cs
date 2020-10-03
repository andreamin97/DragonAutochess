using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public BaseUnit UnitClass;
    
    private Image unitThumbnail;
    private string unitName;
    private int unitCost;

    private PlayerController _playerController;
    private MeshFilter meshFilter;
    private BoardManager boardManager;
    private GameObject currentTile;
    private bool canBeSwapped = true;

    private void Start()
    {
        _playerController = Camera.main.GetComponent<PlayerController>();
        boardManager = FindObjectOfType<BoardManager>();
        meshFilter = GetComponent<MeshFilter>();
        
        Physics.IgnoreLayerCollision(9,9);
        
        InitUnit();
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 1.75f, Quaternion.identity,
            LayerMask.GetMask("Board"));
        foreach (Collider coll in colliders)
        {
            currentTile = coll.gameObject;
            boardManager.SetUnitAtSlot(this.gameObject, coll.gameObject);
        }
    }

    private void OnMouseDown()
    {
        _playerController.selectedUnit = this.gameObject;
        canBeSwapped = false;

    }

    private void OnMouseDrag()
    {
        _playerController.isDragging = true;
        float x = Mathf.Lerp(transform.position.x, _playerController.selectedTile.transform.position.x, 0.75f);
        float z = Mathf.Lerp(transform.position.z, _playerController.selectedTile.transform.position.z, 0.75f);
        float y = _playerController.selectedTile.transform.position.y + 1f;
        transform.position = new Vector3(x, y, z) ;
    }

    private void OnMouseUp()
    {
        if (_playerController.isDragging)
        {
            _playerController.isDragging = false;
            
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 1.75f, Quaternion.identity,
                LayerMask.GetMask("Minis"));
            foreach (Collider coll in colliders)
            {
                coll.GetComponent<Unit>().Swap(currentTile.transform.position);
            }
        }

        canBeSwapped = true;
    }

    public void InitUnit()
    {
        unitName = UnitClass.Name;
        meshFilter.mesh = UnitClass.Mesh;
    }

    public void Swap(Vector3 location)
    {
        if(canBeSwapped)
            transform.position = location + Vector3.up;
    }

    public GameObject GetCurretTile()
    {
        return currentTile;
    }
}
