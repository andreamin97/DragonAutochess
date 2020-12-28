using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public Canvas canvas;

    public void Hide()
    {
        var dateTime = DateTime.Now;

        var id = dateTime.ToString().Replace("/", "").Replace(" ", "").Replace(":", "");

        PlayerPrefs.SetString("MatchID", id);
        SceneManager.LoadScene(1);
    }
}