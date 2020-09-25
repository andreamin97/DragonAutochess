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

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
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
    }

    public void ToggleSelected()
    {
        selected = !selected;
    }
}
