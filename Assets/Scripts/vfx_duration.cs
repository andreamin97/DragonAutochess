using UnityEngine;

public class vfx_duration : MonoBehaviour
{
    public float duration = 5f;

    private PlayerController pc;

    private float timer = 0f;
    // Update is called once per frame

    private void Start()
    {
        Destroy(gameObject, duration);
        pc = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (!pc.isFighting)
            Destroy(gameObject);
    }
}