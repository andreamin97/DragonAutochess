using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Canvas menuCanvas;

    private void Start()
    {
        menuCanvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) menuCanvas.enabled = !menuCanvas.enabled;
    }
}