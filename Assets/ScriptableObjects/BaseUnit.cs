using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Units/Unit")]
public class BaseUnit : ScriptableObject
{
    public enum Range
    {
        Melee,
        Ranged
    }

    public enum Role
    {
        Tank,
        Damage,
        Support
    }

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
    public float MovementSpeed;
    public float AbilityPower;
    public Mesh Mesh;
    public Image Thumbnail;
    public int Cost;
    public AIProfile _aiProfile;
    public Ability ability1;
    public Range range;
    public Role role;
}