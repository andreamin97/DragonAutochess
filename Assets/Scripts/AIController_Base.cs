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

   public IEnumerator AttackAnim()
   {
      float timer = 0f;

      while (timer <= 0.25f)
      {
         transform.Rotate(new Vector3 (0f, 0f, 2f) * Time.deltaTime);
         timer += Time.deltaTime;
      }

      timer = 0f;
      while (timer < 0.25f)
      {
         transform.Rotate(new Vector3 (0f, 0f, -2f) * Time.deltaTime);
         timer += Time.deltaTime;
      }

      yield return null;
   }
}
