using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum GameState {MainMenu, Level1, Level2, Level3, GameOver}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance ??= FindFirstObjectByType<GameManager>();
    
    private GameObject gameOverScreen;
    private GameObject pauseScreen;
    private GameState gameState;

    public void Awake()
    {
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
        gameState = GameState.Level1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pauseScreen = GameObject.FindWithTag("PauseScreen");
        gameOverScreen = GameObject.FindWithTag("GameOver");
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;// should be changed
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
        Time.timeScale = 1f;
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

    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseScreen.SetActive(true);
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseScreen.SetActive(false);
    }

    public GameState GetLevel()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                return GameState.Level1;
            case "Level2":
                return GameState.Level2;
            case "Level3":
                return GameState.Level3;
            default:
                return GameState.GameOver;
        }
    }
}
