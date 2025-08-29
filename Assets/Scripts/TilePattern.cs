using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Pattern", menuName = "Tilemap/Tile Pattern")]
public class TilePattern : ScriptableObject
{
    [System.Serializable]
    public struct TilePlacement
    {
        public Vector3Int relativePosition;
        public TileBase tile;
    }

    public TilePlacement[] tiles;
}