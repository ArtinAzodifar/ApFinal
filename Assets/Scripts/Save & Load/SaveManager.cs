using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public bool IsGameLoaded { get; private set; }
    public int CurrentSlotIndex { get; private set; } = -1;

    private GameData gameData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private string GetSaveFilePath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");
    }

    public bool DoesSlotExist(int slotIndex)
    {
        return File.Exists(GetSaveFilePath(slotIndex));
    }

    public GameData GetSlotData(int slotIndex)
    {
        if (!DoesSlotExist(slotIndex))
        {
            return null;
        }
        string json = File.ReadAllText(GetSaveFilePath(slotIndex));
        GameData data = JsonUtility.FromJson<GameData>(json);
        return data;
    }

    public void SelectSlot(int slotIndex)
    {
        CurrentSlotIndex = slotIndex;
        IsGameLoaded = DoesSlotExist(slotIndex);
    }

    public void NewGame()
    {
        if (CurrentSlotIndex == -1) return;

        this.gameData = new GameData();
        gameData.currentLevelSceneName = "LevelOne";
        gameData.player1_health = 100;
        gameData.player2_health = 100;

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(GetSaveFilePath(CurrentSlotIndex), json);

        IsGameLoaded = true;
        SceneManager.LoadScene(gameData.currentLevelSceneName);
    }

    public void LoadGame()
    {
        if (CurrentSlotIndex == -1 || !IsGameLoaded) return;
        
        string json = File.ReadAllText(GetSaveFilePath(CurrentSlotIndex));
        this.gameData = new GameData();
        JsonUtility.FromJsonOverwrite(json, this.gameData);

        if (SceneManager.GetActiveScene().name != gameData.currentLevelSceneName)
        {
            SceneManager.LoadScene(gameData.currentLevelSceneName);
        }
        else
        {
            ApplyGameData();
        }
    }

    public void SaveGame()
    {
        if (CurrentSlotIndex == -1) return;

        gameData.currentLevelSceneName = SceneManager.GetActiveScene().name;
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0 && players[0] != null)
        {
            gameData.player1_health = players[0].GetComponent<PlayerHealth>().getHealth();
            gameData.player1_position = players[0].transform.position;
            gameData.player1_rotation = players[0].transform.rotation;
        }
        
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(GetSaveFilePath(CurrentSlotIndex), json);
    }

    private void ApplyGameData()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0 && players[0] != null)
        {
            players[0].transform.position = gameData.player1_position;
            players[0].transform.rotation = gameData.player1_rotation;
            players[0].GetComponent<PlayerHealth>().setHealth(gameData.player1_health);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsGameLoaded)
        {
            ApplyGameData();
        }
    }
    
    public void RegisterObjectProcessed(string objectID)
    {
        if (gameData != null && !gameData.DeactiveObjectIDs.Contains(objectID))
        {
            gameData.DeactiveObjectIDs.Add(objectID);
        }
    }

    public bool IsObjectProcessed(string objectID)
    {
        if (gameData == null || gameData.DeactiveObjectIDs == null)
        {
            return false;
        }
        return gameData.DeactiveObjectIDs.Contains(objectID);
    }
    
    public void SetWorldSeed(int seed)
    {
        if (gameData != null)
        {
            gameData.level1_worldSeed = seed;
        }
    }

    public int GetWorldSeed()
    {
        return (gameData != null) ? gameData.level1_worldSeed : 0;
    }
    
    public void DeleteSlot(int slotIndex)
    {
        string filePath = GetSaveFilePath(slotIndex);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        if (CurrentSlotIndex == slotIndex)
        {
            CurrentSlotIndex = -1;
            IsGameLoaded = false;
            gameData = null;
        }
    }
}