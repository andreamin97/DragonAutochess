using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_Base : MonoBehaviour
{
    protected GameObject attackFX;
    public BaseUnit.Range Range;
   
    public struct abilityStruct
    {
        public Ability ability;
        public float coolDown;
        public bool isCasting;

        public abilityStruct(Ability abi, float cD, bool casting)
        {
            ability = abi;
            coolDown = cD;
            isCasting = casting;
        }
    }
   
    public List<abilityStruct> abilityList;

    private void Awake()
    {
        attackFX = (GameObject)Resources.Load("VFX/FX_BloodExplosion_AB");
        abilityList = new List<abilityStruct>();

        var abilities = GetComponents<Ability>();

        foreach (var abi in abilities)
        {
            abilityStruct temp = new abilityStruct(abi, abi.coolDown, false);
            abilityList.Add(temp);
        }
    }
   
   
   
}