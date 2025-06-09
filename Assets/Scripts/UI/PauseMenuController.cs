using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
        
        if (resumeButton != null)
            resumeButton.onClick.AddListener(GameManager.Instance.ResumeGame);

        if (restartButton != null)
            restartButton.onClick.AddListener(GameManager.Instance.Restart);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GameManager.Instance.MainMenu);
    }
}