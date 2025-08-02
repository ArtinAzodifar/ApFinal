using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string currentLevelSceneName;
    
    // Player Data
    public int player1_health;
    public int player1_mana;
    public Vector3 player1_position;
    public Quaternion player1_rotation;
    
    public int player2_health;
    public int player2_mana;
    public Vector3 player2_position;
    public Quaternion player2_rotation;
    
    // Level Data
    public List<string> DeactiveObjectIDs = new List<string>();

    public int level1_worldSeed;
    public int level1_currentChunkIndex;

    public int level2_currentRoomIndex;
    public bool[] level2_clearedRooms;

    public bool level3_hasReachedBoss;
}