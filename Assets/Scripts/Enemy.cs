using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public BaseEnemy enemy;
    
    private string unitName;
    private MeshFilter meshFilter;

    private void Start()
    {
        InitUnit();
    }

    public void InitUnit()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = enemy.Mesh;
    }
}
