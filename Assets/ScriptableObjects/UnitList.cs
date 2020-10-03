using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Units/Unit List")]
public class UnitList : ScriptableObject
{
    public BaseUnit[] Units;

    public BaseUnit RandomUnit()
    {
        return Units[Random.Range(0, Units.Length)];
    }
}
