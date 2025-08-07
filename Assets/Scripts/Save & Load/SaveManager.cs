using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public GameData LoadedData { get { return gameData; } }
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
        if (!DoesSlotExist(slotIndex)) return null;
        string json = File.ReadAllText(GetSaveFilePath(slotIndex));
        return JsonUtility.FromJson<GameData>(json);
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
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(GetSaveFilePath(CurrentSlotIndex), json);
        IsGameLoaded = true;
        SceneManager.LoadScene("LevelThree");
    }

    public void LoadGame()
    {
        if (CurrentSlotIndex == -1 || !IsGameLoaded) return;
        string json = File.ReadAllText(GetSaveFilePath(CurrentSlotIndex));
        this.gameData = JsonUtility.FromJson<GameData>(json);
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
        if (CurrentSlotIndex == -1 || gameData == null) return;
        gameData.currentLevelSceneName = SceneManager.GetActiveScene().name;

        GameObject player1 = GameObject.FindWithTag("Player1");
        if (player1 != null)
        {
            PlayerHealth p1Health = player1.GetComponent<PlayerHealth>();
            gameData.player1_health = p1Health.getHealth();
            gameData.player1_lives = p1Health.getLives();
            gameData.player1_mana = player1.GetComponent<Player1SuperAttack>().getMana();
            gameData.player1_position = player1.transform.position;
            gameData.player1_rotation = player1.transform.rotation;
        }

        GameObject player2 = GameObject.FindWithTag("Player2");
        if (player2 != null)
        {
            PlayerHealth p2Health = player2.GetComponent<PlayerHealth>();
            gameData.player2_health = p2Health.getHealth();
            gameData.player2_lives = p2Health.getLives();
            gameData.player2_mana = player2.GetComponent<P2SuperShoot>().getMana();
            gameData.player2_position = player2.transform.position;
            gameData.player2_rotation = player2.transform.rotation;
        }

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(GetSaveFilePath(CurrentSlotIndex), json);
    }

    private void ApplyGameData()
    {
        if (gameData == null) return;

        GameObject player1 = GameObject.FindWithTag("Player1");
        if (player1 != null)
        {
            player1.transform.position = gameData.player1_position;
            player1.transform.rotation = gameData.player1_rotation;
            player1.GetComponent<PlayerHealth>().LoadHealth(gameData.player1_health, gameData.player1_lives);
            player1.GetComponent<Player1SuperAttack>().setMana(gameData.player1_mana);
        }

        GameObject player2 = GameObject.FindWithTag("Player2");
        if (player2 != null)
        {
            player2.transform.position = gameData.player2_position;
            player2.transform.rotation = gameData.player2_rotation;
            player2.GetComponent<PlayerHealth>().LoadHealth(gameData.player2_health, gameData.player2_lives);
            player2.GetComponent<P2SuperShoot>().setMana(gameData.player2_mana);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsGameLoaded && (scene.name == "LevelOne" || scene.name == "LevelTwo" || scene.name == "LevelThree"))
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
        if (gameData == null || gameData.DeactiveObjectIDs == null) return false;
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