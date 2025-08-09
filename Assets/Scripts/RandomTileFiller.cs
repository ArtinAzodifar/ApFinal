using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PatternPlacer : MonoBehaviour
{
    public Tilemap targetTilemap;
    public TilePattern patternToPlace;
    public Vector3Int cornerA;
    public Vector3Int cornerB;
    public int numberOfPatterns;

#if UNITY_EDITOR
    private static bool hasLoggedPattern = false; // Prevents spamming the log

    [ContextMenu("Place Random Patterns")]
    private void PlacePatterns()
    {
        if (targetTilemap == null || patternToPlace == null || patternToPlace.tiles.Length == 0)
        {
            Debug.LogError("Target Tilemap, Pattern, or the pattern's tile list is not set!");
            return;
        }

        // --- DEBUGGING STEP: Log the pattern contents once ---
        if (!hasLoggedPattern)
        {
            Debug.Log($"--- Checking pattern asset '{patternToPlace.name}' ---");
            foreach (var placement in patternToPlace.tiles)
            {
                string tileName = (placement.tile != null) ? placement.tile.name : "null";
                Debug.Log($"Pattern Tile: [Pos: {placement.relativePosition}, Tile: {tileName}]");
            }
            hasLoggedPattern = true;
        }
        // --- END DEBUGGING STEP ---

        int minX = Mathf.Min(cornerA.x, cornerB.x);
        int maxX = Mathf.Max(cornerA.x, cornerB.x);
        int minY = Mathf.Min(cornerA.y, cornerB.y);
        int maxY = Mathf.Max(cornerA.y, cornerB.y);
        
        int patternsPlaced = 0;
        int placementAttempts = 0;
        int maxAttempts = numberOfPatterns * 200;

        int patternSize = patternToPlace.tiles.Length;
        Vector3Int[] positionArray = new Vector3Int[patternSize];
        TileBase[] tileArray = new TileBase[patternSize];

        while (patternsPlaced < numberOfPatterns && placementAttempts < maxAttempts)
        {
            placementAttempts++;
            
            int randomX = Random.Range(minX, maxX + 1);
            int randomY = Random.Range(minY, maxY + 1);
            Vector3Int anchorPosition = new Vector3Int(randomX, randomY, 0);

            if (CanPlacePattern(anchorPosition))
            {
                for(int i = 0; i < patternSize; i++)
                {
                    positionArray[i] = anchorPosition + patternToPlace.tiles[i].relativePosition;
                    tileArray[i] = patternToPlace.tiles[i].tile;
                }
                
                targetTilemap.SetTiles(positionArray, tileArray);
                
                patternsPlaced++;
            }
        }
        
        EditorUtility.SetDirty(targetTilemap);
        Debug.Log($"Placed {patternsPlaced} out of {numberOfPatterns} requested patterns.");
    }

    private bool CanPlacePattern(Vector3Int anchorPosition)
    {
        foreach (var placement in patternToPlace.tiles)
        {
            Vector3Int tilePos = anchorPosition + placement.relativePosition;
            if (targetTilemap.HasTile(tilePos))
            {
                return false;
            }
        }
        return true;
    }
#endif
}