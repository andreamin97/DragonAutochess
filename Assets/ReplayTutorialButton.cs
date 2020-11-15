using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayTutorialButton : MonoBehaviour
{
    public void ResetTutorial()
    {
        PlayerPrefs.SetInt("hasPlayedTutorial", 0);
        FindObjectOfType<TutorialManager>().PlayTutorial();
    }
}
