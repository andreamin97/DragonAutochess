using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_Base : MonoBehaviour
{
   public Ability ability1;
   protected GameObject attackFX;
   public BaseUnit.Range Range;

   private void Awake()
   {
      attackFX = (GameObject)Resources.Load("VFX/FX_BloodExplosion_AB");
   }
   
   
   
}
