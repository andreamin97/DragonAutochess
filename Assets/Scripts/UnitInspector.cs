using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInspector : MonoBehaviour
{
    public Text unitName;
    public Text unitCurrentHp;
    public Text unitMaxHp;

    public Text ad;
    public Text atkspd;
    public Text armor;
    public Text leech;
    public Text abilityText;
    
    public Slider healthBar;
    public Canvas canvas;

    private GameObject oldUnit = null;
    private bool canDisable = true;

    private PlayerController _playerController;
    
    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {

        if (_playerController.selectedUnit != null)
        {
            var unit = _playerController.selectedUnit.GetComponent<Unit>();

            unitName.text = unit.unitName;
            unitCurrentHp.text = unit.currentHealth.ToString();
            unitMaxHp.text = unit.maxHealth.ToString();
            healthBar.value = unit.currentHealth / unit.maxHealth;
            ad.text = unit._attackDamage.ToString();
            atkspd.text = unit._attackSpeed.ToString();
            leech.text = unit.leech.ToString();
            armor.text = unit.armor.ToString();
            abilityText.text = unit.GetComponent<AIController_Base>().ability1.abilityText;


        }
        
    }

    public void Hide()
    {
        if (canDisable)
        {
            canvas.enabled  = false;
        }
        else
        {
            canDisable = true;
        }
        
    }

    public void Show()
    {
        canvas.enabled = true;
        canDisable = false;
    }
}