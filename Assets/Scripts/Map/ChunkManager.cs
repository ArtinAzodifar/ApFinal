using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("Chunk Settings")]
    [SerializeField] private int chunkWidth;
    [SerializeField] private float generateAheadDistance;
    [SerializeField] private int initialChunks;

    [Header("References")]
    [SerializeField] private GameObject[] chunkPrefabs1;
    [SerializeField] private GameObject[] chunkPrefabs2;
    [SerializeField] private GameObject[] players;

    private int chunkWorldWidth;
    private int totalChunksGenerated = 0;

    [SerializeField] private int numOfChunks1 = 5;
    private int chunks1Counter = 0;
    private bool type1Ended = false;

    [SerializeField] private int numOfChunks2 = 5;
    private int chunks2Counter = 0;

    void Start()
    {
        chunkWorldWidth = chunkWidth;

        for (int i = 0; i < initialChunks; i++)
        {
            GameObject newChunk = GenerateChunk(i);
            totalChunksGenerated++;
        }
    }

    void Update()
    {
        float furthestPlayerX = float.MinValue;

        foreach (GameObject player in players)
        {
            if (player.transform.position.x > furthestPlayerX)
            {
                furthestPlayerX = player.transform.position.x;
            }
        }

        float furthestChunkEndX = totalChunksGenerated * chunkWorldWidth;

        if (furthestPlayerX + generateAheadDistance > furthestChunkEndX)
        {
            GameObject newChunk = GenerateChunk(totalChunksGenerated);
            if (newChunk != null)
            {
                totalChunksGenerated++;
            }
        }
    }

    GameObject GenerateChunk(int chunkIndex)
    {
        GameObject selectedChunkPrefab = null;
        int randomChunkIndex;
        float positionY = 0f;

        if (chunks1Counter < numOfChunks1)
        {
            randomChunkIndex = Random.Range(0, chunkPrefabs1.Length);
            selectedChunkPrefab = chunkPrefabs1[randomChunkIndex];
            chunks1Counter++;
        }
        else if (!type1Ended)
        {
            type1Ended = true;
            totalChunksGenerated++;
            return null;
        }
        else if (chunks2Counter < numOfChunks2)
        {
            randomChunkIndex = Random.Range(0, chunkPrefabs2.Length);
            selectedChunkPrefab = chunkPrefabs2[randomChunkIndex];
            chunks2Counter++;
            positionY = -100f;
        }
        else
        {
            return null;
        }

        GameObject chunk = Instantiate(selectedChunkPrefab, transform);
        chunk.name = "Chunk_1_" + chunkIndex + "_Type_" + randomChunkIndex;

        float positionX = chunkIndex * chunkWorldWidth;
        chunk.transform.position = new Vector3(positionX, positionY, 0);

        return chunk;
    }
}
