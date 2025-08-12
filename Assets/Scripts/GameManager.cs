using System;
using System.Collections;
using PlayFab.SharedModels;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameObject gameOverScreen;
    private GameObject pauseScreen;

    private PauseMenuController pauseMenuController;
    private NetworkVariable<bool> keyFound = new NetworkVariable<bool>(false);

    public GameObject audioControllerPrefab;
    public GameObject musicPlayerPrefab;
    private bool isLocalMode;

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
        isLocalMode = NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening;
    }

    //TODO
    private void InitializeAudioSystems()
    {
        if (AudioController.Instance == null && audioControllerPrefab != null)
            Instantiate(audioControllerPrefab);

        if (FindFirstObjectByType<MusicPlayer>() == null && musicPlayerPrefab != null)
            Instantiate(musicPlayerPrefab);
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Key.KeyCollected += FindKey;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Key.KeyCollected -= FindKey;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        pauseScreen = GameObject.FindWithTag("PauseScreen");
        gameOverScreen = GameObject.FindWithTag("GameOver");
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        if (pauseScreen != null)
        {
            pauseMenuController = pauseScreen.GetComponent<PauseMenuController>();
            pauseScreen.SetActive(false);
        }
        if (!isLocalMode && scene.name.Contains("Level") && IsServer) StartCoroutine(assignPlayers());
    }

    public void GameOver()
    {
        if (isLocalMode)
        {
            applyGameOver();
            return;
        }
        //online mode
        if (!IsServer) return;
        //ask all clients to apply gameOver
        gameOverClientRpc();
    }
    [ClientRpc]
    private void gameOverClientRpc()
    {
        applyGameOver();
    }
    private void applyGameOver()
    {
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //level set
    public void StartGame()
    {
        if (isLocalMode) SceneManager.LoadScene("LevelOne", LoadSceneMode.Single);
        //online mode
        else NetworkManager.Singleton.SceneManager.LoadScene("LevelOne", LoadSceneMode.Single);
    }
    public void Level2()
    {
        SaveManager.Instance.IsChangingLevel = true;
        SaveManager.Instance.SetNextLevel("LevelTwo");
        SaveManager.Instance.SaveGame();
        
        if (isLocalMode) SceneManager.LoadScene("LevelTwo", LoadSceneMode.Single);
        //online mode
        else NetworkManager.Singleton.SceneManager.LoadScene("LevelTwo", LoadSceneMode.Single);
    }
    public void Level3()
    {    
        SaveManager.Instance.IsChangingLevel = true;
        SaveManager.Instance.SetNextLevel("LevelThree");
        SaveManager.Instance.SaveGame();
        
        if (isLocalMode) SceneManager.LoadScene("LevelThree", LoadSceneMode.Single);
        //online mode
        else NetworkManager.Singleton.SceneManager.LoadScene("LevelThree", LoadSceneMode.Single);
    }

    //assign characters to their owner in online mode
    private IEnumerator assignPlayers()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.IsListening);

        if (!IsServer) yield break;

        //give the ownerShip of each character to its player
        foreach (var player in CharSelector.Instance.players)
        {
            GameObject scenePlayer = player.characterID == 0 ? GameObject.FindWithTag("Player1") : GameObject.FindWithTag("Player2");
            if (scenePlayer == null) continue;
            yield return new WaitUntil(() => scenePlayer.GetComponent<NetworkObject>().IsSpawned == true);
            scenePlayer.GetComponent<NetworkObject>().ChangeOwnership(player.clientID);
        }
    }

    public void Restart()
    {
        if (isLocalMode)
        {
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        if (!IsServer) return;
        restartClientRpc(SceneManager.GetActiveScene().buildIndex);
    }
    [ClientRpc]
    private void restartClientRpc(int sceneIndex)
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(sceneIndex);
    }

    public void MainMenu()
    {
        // the important call to save the game!
        SaveManager.Instance.SaveGame();

        //in MainMenu we don't have network yet
        if (!isLocalMode && IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            NetworkManager.Singleton.Shutdown();
        }
        else if (isLocalMode)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        Instance.isLocalMode = true;
    }

    public void LoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }
    public void Lobby()
    {
        Instance.isLocalMode = false;
        SceneManager.LoadScene("LobbyScene");
    }
    public void Quit()
    {
        Application.Quit();
    }

    //in online mode, only host can pause/resume the game
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
        if (isLocalMode)
        {
            applyPause();
            return;
        }
        if (IsServer) pauseClientRpc();
        else if (IsClient) pauseServerRpc();

    }
    [ServerRpc]
    private void pauseServerRpc()
    {
        pauseClientRpc();
    }

    [ClientRpc]
    private void pauseClientRpc()
    {
        applyPause();
    }

    private void applyPause()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (pauseMenuController != null)
        {
            pauseMenuController.ShowMenu();
        }
    }

    public void ResumeGame()
    {
        if (isLocalMode)
        {
            applyResume();
            return;
        }
        if (IsServer) resumeClientRpc();
        else if (IsClient) resumeServerRpc();
    }
    [ServerRpc]
    private void resumeServerRpc()
    {
        resumeClientRpc();
    }

    [ClientRpc]
    private void resumeClientRpc()
    {
        applyResume();
    }

    private void applyResume()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        if (pauseMenuController != null)
        {
            pauseMenuController.HideMenu();
        }
    }

    public void FindKey()
    {
        if (isLocalMode || IsServer) keyFound.Value = true;
        else requestFindKeyServerRpc();
    }
    [ServerRpc]
    private void requestFindKeyServerRpc()
    {
        keyFound.Value = true;
    }

    //getters
    public bool GetKey()
    {
        return keyFound.Value;
    }
    public bool IsLocalMode()
    {
        return isLocalMode;
    }

    //setter
    public void setKey(bool value)
    {
        keyFound.Value = false;
    }
}