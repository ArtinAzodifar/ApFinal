using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private GameData gameData;
    private string saveFilePath;

    [SerializeField] private ChunkManager chunkManager;
    [SerializeField] private GameObject[] players;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        this.gameData.level1_worldSeed = Random.Range(int.MinValue, int.MaxValue);

        if (chunkManager != null)
        {
            // chunkManager.InitializeLevel(this.gameData.level1_worldSeed);
        }
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            this.gameData = new GameData();
            JsonUtility.FromJsonOverwrite(json, this.gameData);
            
            if (players.Length > 0 && players[0] != null)
            {
                players[0].transform.position = gameData.player1_position;
                players[0].transform.rotation = gameData.player1_rotation;
                players[0].GetComponent<PlayerHealth>().setHealth(gameData.player1_health);
                players[0].GetComponent<Player1SuperAttack>().setMana(gameData.player1_mana);
            }
            if (players.Length > 1 && players[1] != null)
            {
                players[1].transform.position = gameData.player2_position;
                players[1].transform.rotation = gameData.player2_rotation;
                players[1].GetComponent<PlayerHealth>().setHealth(gameData.player1_health);
                players[1].GetComponent<P2SuperShoot>().setMana(gameData.player1_mana);
            }
            
            if (chunkManager != null)
            {
                // chunkManager.InitializeLevel(this.gameData.level1_worldSeed);
            }
        }
        else
        {
            NewGame();
        }
    }

    public void SaveGame()
    {
        if (players.Length > 0 && players[0] != null)
        {
            // Player 1
            gameData.player1_health = players[0].GetComponent<PlayerHealth>().getHealth();
            gameData.player1_mana = players[0].GetComponent<Player1SuperAttack>().getMana();
            gameData.player1_position = players[0].transform.position;
            gameData.player1_rotation = players[0].transform.rotation;
        }
        if (players.Length > 1 && players[1] != null)
        {
            // Player 2
            gameData.player2_health = players[1].GetComponent<PlayerHealth>().getHealth();
            gameData.player2_mana = players[1].GetComponent<P2SuperShoot>().getMana();
            gameData.player2_position = players[1].transform.position;
            gameData.player2_rotation = players[1].transform.rotation;
        }

        string json = JsonUtility.ToJson(this.gameData, true);
        File.WriteAllText(saveFilePath, json);
    }
    
    public void RegisterObjectProcessed(string objectID)
    {
        if (!gameData.DeactiveObjectIDs.Contains(objectID))
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
}