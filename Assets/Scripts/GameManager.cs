using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum GameState {MainMenu, Level1, Level2, Level3, GameOver}

public class GameManager : MonoBehaviour
{
    // private static GameManager instance;
    public static GameManager Instance { get; private set; }
    
    private GameObject gameOverScreen;
    private GameObject pauseScreen;
    private GameState gameState;
    private bool level1keyFound = false;
    
    public GameObject audioControllerPrefab;
    public GameObject musicPlayerPrefab;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSystems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSystems()
    {
        if (AudioController.Instance == null && audioControllerPrefab != null)
            Instantiate(audioControllerPrefab);

        if (FindObjectOfType<MusicPlayer>() == null && musicPlayerPrefab != null)
            Instantiate(musicPlayerPrefab);
    }


    public void Start()
    {
        gameState = GameState.MainMenu;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Key.KeyCollected += FindKey1;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Key.KeyCollected -= FindKey1;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pauseScreen = GameObject.FindWithTag("PauseScreen");
        gameOverScreen = GameObject.FindWithTag("GameOver");
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
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
        SceneManager.LoadScene("LevelOne");
    }

    public void NextLevel()
    {
        switch (gameState)
        {
            case GameState.Level1:
                gameState = GameState.Level2;
                SceneManager.LoadScene("LevelTwo");
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
        Cursor.visible = true;
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
        Cursor.visible = true;
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

    public void FindKey1()
    {
        level1keyFound = true;
    }

    public bool GetKey1()
    {
        return level1keyFound;
    }
}
