using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Floating_Text : MonoBehaviour
{
    public float time = .75f;
    public Color textColor;

    private void Start()
    {
        GetComponent<Text>().color = textColor;
        Destroy(this.gameObject, time);
    }
}
