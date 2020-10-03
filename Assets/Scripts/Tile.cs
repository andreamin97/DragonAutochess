﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    public Material selectedMaterial;
    public Material material;
    
    private MeshRenderer mr;
    private bool selected = false;
    private PlayerController playerController;
    private BoardManager bm;
    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        bm = FindObjectOfType<BoardManager>();
        playerController = Camera.main.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (selected)
        {
            mr.material = selectedMaterial;
        }
        else
        {
            mr.material = material;
        }

        if (Physics.CheckBox(transform.position, transform.localScale / 1.75f, Quaternion.identity,
            LayerMask.GetMask("Minis")))
            bm.SetUnitAtSlot(null, this.gameObject);
    }

    public void ToggleSelected()
    {
        selected = !selected;
    }

}
