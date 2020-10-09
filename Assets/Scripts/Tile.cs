using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    public GameObject selectedOutline;
    
    private MeshRenderer mr;
    private bool selected = false;
    private PlayerController _playerController;
    private BoardManager bm;
    private void Start() {
        mr = GetComponent<MeshRenderer>();
        bm = FindObjectOfType<BoardManager>();
        _playerController = Camera.main.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (selected)
        {
            selectedOutline.SetActive(true);
        }
        else
        {
            selectedOutline.SetActive(false);
        }

        if (Physics.CheckBox(transform.position, transform.localScale / 1.75f, Quaternion.identity,
            LayerMask.GetMask("Minis")) && !_playerController.isFighting)
            bm.SetUnitAtSlot(null, this.gameObject);
    }

    public void ToggleSelected()
    {
        selected = !selected;
    }

}
