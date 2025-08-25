using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuController : MonoBehaviour
{
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(GameManager.Instance.Restart);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(() => GameManager.Instance.MainMenu(false));
        }
    }
}