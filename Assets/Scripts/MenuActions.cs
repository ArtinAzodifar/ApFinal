using UnityEngine;

public class MenuActions : MonoBehaviour
{
    public void LoadMainMenu()
    {
        GameManager.Instance.MainMenu(false);
    }
    
    public void LoadLoginScene()
    {
        GameManager.Instance.LoginScene();
    }
    
    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}