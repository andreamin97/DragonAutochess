using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellUnuit : MonoBehaviour
{
    private BoardManager boardManager;
    private PlayerController _playerController;

    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Physics.CheckBox(transform.position, transform.localScale * 1.5f, Quaternion.identity,
            LayerMask.GetMask("Minis")) && !_playerController.isDragging)
       {
           boardManager.RemoveUnit(_playerController.selectedUnit, true);
       }
    }
}
