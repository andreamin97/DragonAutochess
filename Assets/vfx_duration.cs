using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfx_duration : MonoBehaviour
{
    public float duration = 5f;

    private float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (timer >= duration)
            Destroy(this.gameObject);

        timer += Time.deltaTime;
    }
}
