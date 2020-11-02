using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    public Canvas _canvas;
    public Slider _slider;
    public Unit _unit;
    public Text status;
    public Image statusDuration;

    // Start is called before the first frame update
    void Start()
    {
        //_canvas = GetComponent<Canvas>();
        //_slider = GetComponent<Slider>();
        //_unit = GetComponent<PlayerUnit>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _canvas.transform.rotation = Quaternion.Euler(15f, 90f, 0f);
        _slider.value = _unit.currentHealth / _unit.maxHealth;

        switch (_unit.currentStatus)
        {
            case Unit.Statuses.None:
                status.text = " ";
                statusDuration.fillAmount = 0f;
                break;
            case Unit.Statuses.Snared:
                status.text = "Snared";
                statusDuration.fillAmount = _unit._conditionDuration / _unit._conditionMaxDuration;
                break;
        }
        
    }
}