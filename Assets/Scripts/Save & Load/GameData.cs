using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string currentLevelSceneName;

    public int player1_health;
    public int player1_lives;
    public int player1_mana;
    public Vector3 player1_position;
    public Quaternion player1_rotation;

    public int player2_health;
    public int player2_lives;
    public int player2_mana;
    public Vector3 player2_position;
    public Quaternion player2_rotation;

    public List<string> DeactiveObjectIDs = new List<string>();
    public int level1_worldSeed;

    public GameData()
    {
        currentLevelSceneName = "LevelOne";

        player1_health = 150;
        player1_lives = 3;
        player1_mana = 0;
        player1_position = new Vector3(-201.7082f, -17.64f, 0f);
        player1_rotation = Quaternion.identity;

        player2_health = 100;
        player2_lives = 3;
        player2_mana = 0;
        player2_position = new Vector3(-203.65f, -17.64f, 0f);
        player2_rotation = Quaternion.identity;
    }
}