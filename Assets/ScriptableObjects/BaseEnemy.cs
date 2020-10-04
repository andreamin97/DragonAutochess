﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 [CreateAssetMenu (menuName = "Units/Enemy")]
public class BaseEnemy : ScriptableObject
{
     [SerializeField] public string Name;
     [SerializeField] public float Health;
     [SerializeField] public float Armor;
     [SerializeField] public float MagicResist;
     [SerializeField] public float AttackDamage;
     [SerializeField] public float AbilityPower;
     [SerializeField] public Mesh Mesh;
     [SerializeField] public Image Thumbnail;
     [SerializeField] public int Cost;

}
