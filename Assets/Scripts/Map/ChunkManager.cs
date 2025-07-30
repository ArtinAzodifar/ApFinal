using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    private int worldSeed;
    
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

    [SerializeField] private int numOfChunks1 = 4;
    private int fixIndex = 1;
    private int lastIndexChunk1 = -1; 
    private int chunks1Counter = 0;
    private bool type1Ended = false;

    [SerializeField] private int numOfChunks2 = 3;
    private int lastIndexChunk2 = -1;
    private int chunks2Counter = 0;

    void Start()
    {
        // chunkWorldWidth = chunkWidth;
        //
        // for (int i = 0; i < initialChunks; i++)
        // {
        //     GameObject newChunk = GenerateChunk(i);
        //     totalChunksGenerated++;
        // }
    }
    
    public void InitializeLevel(int seed)
    {
        this.worldSeed = seed;
        Random.InitState(this.worldSeed);
        
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
            while (lastIndexChunk1 == randomChunkIndex || randomChunkIndex == fixIndex)
            {
                randomChunkIndex = Random.Range(0, chunkPrefabs1.Length);
            }
            lastIndexChunk1 = randomChunkIndex;
            selectedChunkPrefab = chunkPrefabs1[randomChunkIndex];
            chunks1Counter++;
        }
        else if (!type1Ended)
        {
            selectedChunkPrefab = chunkPrefabs1[fixIndex];
            type1Ended = true;
            totalChunksGenerated++;
            //return null;
        }
        else if (chunks2Counter < numOfChunks2)
        {
            randomChunkIndex = Random.Range(0, chunkPrefabs2.Length);
            while (lastIndexChunk2 == randomChunkIndex)
            {
                randomChunkIndex = Random.Range(0, chunkPrefabs2.Length);
            }
            lastIndexChunk2 = randomChunkIndex;
            selectedChunkPrefab = chunkPrefabs2[randomChunkIndex];
            chunks2Counter++;
            positionY = -100f;
        }
        else
        {
            return null;
        }

        GameObject chunk = Instantiate(selectedChunkPrefab, transform);
        //chunk.name = "Chunk_1_" + chunkIndex + "_Type_" + randomChunkIndex;

        float positionX = chunkIndex * chunkWorldWidth;
        chunk.transform.position = new Vector3(positionX, positionY, 0);

        return chunk;
    }
}
