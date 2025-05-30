using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum GameState {MainMenu, Level1, Level2, Level3, Pause, GameOver}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance ??= FindFirstObjectByType<GameManager>();
    
    [SerializeField] private GameObject gameOverScreen;
    private GameState gameState = GameState.MainMenu;

    public void Awake()
    {
        gameOverScreen.SetActive(false);
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        gameState = GameState.Level1;
        SceneManager.LoadScene("Level1");
    }

    public void NextLevel()
    {
        switch (gameState)
        {
            case GameState.Level1:
                gameState = GameState.Level2;
                SceneManager.LoadScene("Level2");
                break;
            case GameState.Level2:
                gameState = GameState.Level3;
                SceneManager.LoadScene("Level3");
                break;
            default:
                gameState = GameState.Level3;
                break;
        }
    }
    
    public void Restart()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        gameState = GameState.MainMenu;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
