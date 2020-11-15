using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfx_duration : MonoBehaviour
{
    public float duration = 5f;

    private float timer = 0f;

    private PlayerController pc;
    // Update is called once per frame

    private void Start()
    {
        Destroy(this.gameObject, duration);
        pc = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (!pc.isFighting)
            Destroy(this.gameObject);
    }
}
