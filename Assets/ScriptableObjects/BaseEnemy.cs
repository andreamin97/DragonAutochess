using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 [CreateAssetMenu (menuName = "Units/Enemy")]
public class BaseEnemy : ScriptableObject
{
     public string Name;
     public float Health;
     public float HpPerLevel;
     public float Armor;
     public float ArmorPerLevel;
     public float MagicResist;
     public float MrPerLevel;
     public float AttackDamage;
     public float ADPerLevel;
     public float AttackRange;
     public float AttackSpeed;
     public float ASPerLevel;
     public float AbilityPower;
     public float MovementSpeed;
     public Mesh Mesh;
     public Image Thumbnail;
     public AIProfile _aiProfile;
}
