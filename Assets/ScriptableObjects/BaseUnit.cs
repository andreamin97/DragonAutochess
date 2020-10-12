using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 [CreateAssetMenu (menuName = "Units/Unit")]
public class BaseUnit : ScriptableObject
{
     public string Name;
     public float Health;
     public float Armor;
     public float MagicResist;
     public float AttackDamage;
     public float AttackRange;
     public float AttackSpeed;
     public float MovementSpeed;
     public float AbilityPower;
     public Mesh Mesh;
     public Image Thumbnail;
     public int Cost;
     public AIProfile _aiProfile;
}
