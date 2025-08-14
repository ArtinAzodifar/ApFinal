using UnityEngine;

public class MenuActions : MonoBehaviour
{
    public void LoadMainMenu()
    {
        GameManager.Instance.MainMenu();
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