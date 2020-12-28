using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public int NextTutorialIndex;
    private TutorialManager _tutorialManager;

    private void Awake()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();
    }

    public void NextTutorial()
    {
        if (_tutorialManager.TutorialIndex == NextTutorialIndex - 1) _tutorialManager.NextTutorial();
    }
}