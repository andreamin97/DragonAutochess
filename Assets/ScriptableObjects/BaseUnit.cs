using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 [CreateAssetMenu (menuName = "Units/Unit")]
public class BaseUnit : ScriptableObject
{
     [SerializeField] public string Name;
     [SerializeField] public float Health;
     [SerializeField] public float Armor;
     [SerializeField] public float MagicResist;
     [SerializeField] public float AttackDamage;
     public float AttackRange;
     public float AttackSpeed;
     public float MovementSpeed;
     [SerializeField] public float AbilityPower;
     [SerializeField] public Mesh Mesh;
     [SerializeField] public Image Thumbnail;
     [SerializeField] public int Cost;
     [SerializeField] public AIProfile _aiProfile;
}
