using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private enum ChunkShapes
    {
        None,
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft,
        Top,
        Right,
        Bottom,
        Left,
        Four

    }

    [Header("Elements")]
    [SerializeField] private Transform world;
    Chunk[,] grid;

    [Header("Settings")]
    private WorldData worldData;
    private string dataPath;
    private bool shouldSave;
    [SerializeField] private int gridSize;
    [SerializeField] private int gridScale;

    [Header("Chunk Meshes")]
    [SerializeField] private Mesh[] chunkShapes;

    private void Awake()
    {
        Chunk.onUnlocked += ChunkUnlockedCallback;
        Chunk.onPriceChanged += ChunkPriceChangeCallBack;

    }
    // Start is called before the first frame update
    void Start()
    {
        dataPath = Application.persistentDataPath + "/WorldData.txt";
        LoadWorld();
        Initialize();

        InvokeRepeating("TrySaveGame", 1, 1);
    }

    private void OnDestroy()
    {
        Chunk.onUnlocked -= ChunkUnlockedCallback;
        Chunk.onPriceChanged -= ChunkPriceChangeCallBack;

    }
    // Update is called once per frame
    void Update()
    {

    }

    private void Initialize()
    {
        for(int i = 0; i <world.childCount;i++)
            world.GetChild(i).GetComponent<Chunk>().Initialize(worldData.chunkPrices[i]);

        InitializeGrid();

        UpdtadeChunkWalls();
        UpdateGridRenderer();
    }

    private void InitializeGrid()
    {
        grid = new Chunk[gridSize,gridSize];

        for(int i = 0; i < world.childCount; i++)
        {
            Chunk chunk = world.GetChild(i).GetComponent<Chunk>();

            Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x/gridScale,
                (int)chunk.transform.position.z / gridScale);

            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);

            grid[chunkGridPosition.x,chunkGridPosition.y]=chunk;
        }
    }
   private void UpdtadeChunkWalls()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];

                if(chunk == null) 
                    continue;

                Chunk frontChunk = IsValidGridPosition(x, y + 1) ? grid[x, y + 1] : null;
                Chunk rightChunk = IsValidGridPosition(x + 1, y) ? grid[x + 1, y] : null;
                Chunk backChunk = IsValidGridPosition(x, y - 1) ? grid[x, y - 1] : null;
                Chunk leftChunk = IsValidGridPosition(x - 1, y) ? grid[x - 1, y] : null;

                int configuration = 0;

                if (frontChunk != null && frontChunk.Isunlocked())
                    configuration = configuration +1;

                if (rightChunk != null && rightChunk.Isunlocked())
                    configuration = configuration +2;

                if (backChunk != null && backChunk.Isunlocked())
                    configuration = configuration + 4;

                if (leftChunk != null && leftChunk.Isunlocked())
                    configuration = configuration +8;

                chunk.UpdateWalls(configuration);

                SetChunRenderer(chunk, configuration);

            }
        }
    }

    private void SetChunRenderer(Chunk chunk, int configuraiton)
    {
        switch (configuraiton)
        {
            case 0:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.Four]);
                break;
            case 1:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.Bottom]);
                break;
            case 2:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.Left]);
                break;
            case 3:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.BottomLeft]);
                break;
            case 4:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.Top]);
                break;
            case 5:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.None]);
                break;
            case 6:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.TopLeft]);
                break;
            case 7:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.None]);
                break;
            case 8:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.Right]);
                break;
            case 9:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.BottomRight]);
                break;
            case 10:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.None]);
                break;
            case 11:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.None]);
                break;
            case 12:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.TopRight]);
                break;
            case 13:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.None]);
                break;
            case 14:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.None]);
                break;
            case 15:
                chunk.SetRenderer(chunkShapes[(int)ChunkShapes.None]);
                break;
        }
    }

    private void UpdateGridRenderer()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];

                if (chunk == null)
                    continue;

                if(chunk.Isunlocked())
                    continue;

                Chunk frontChunk = IsValidGridPosition(x, y+1) ? grid[x,y+1]: null;
                Chunk rightChunk = IsValidGridPosition(x+1, y) ? grid[x+1,y]: null;
                Chunk backChunk = IsValidGridPosition(x, y-1) ? grid[x,y-1]: null;
                Chunk leftChunk = IsValidGridPosition(x-1, y) ? grid[x-1,y]: null;

                if (frontChunk != null && frontChunk.Isunlocked())
                    chunk.DisplayLockedElements();
               else if (rightChunk != null && rightChunk.Isunlocked())
                    chunk.DisplayLockedElements();
               else if (backChunk != null && backChunk.Isunlocked())
                    chunk.DisplayLockedElements();
               else  if (leftChunk != null && leftChunk.Isunlocked())
                    chunk.DisplayLockedElements();
            }
        }
    }
    private bool IsValidGridPosition(int x,int y)
    {
        if(x < 0 || x >= gridSize || y < 0|| y >= gridSize)
            return false;

        return true;    
    }

   private void TrySaveGame()
    {
       // Debug.Log("saving");

        if (shouldSave)
        {
            SaveWorld();
            shouldSave = false;
        }
    }

    private void ChunkUnlockedCallback()
    {
        Debug.Log("Chunk Unlocked");

        UpdtadeChunkWalls();

        UpdateGridRenderer();

        SaveWorld();
    }

    private void ChunkPriceChangeCallBack()
    {
        shouldSave = true;
    }

    private void LoadWorld()
    {
        
        string data = "";

        if(!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath,FileMode.Create);

            worldData = new WorldData();

            for (int i = 0; i < world.childCount; i++)
            {
                int chunkInitialPrice = world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();
                worldData.chunkPrices.Add(chunkInitialPrice);

            }

            string worldDataString = JsonUtility.ToJson(worldData,true);

            byte[]worldDataBytes =Encoding.UTF8.GetBytes(worldDataString);

            fs.Write(worldDataBytes);

            fs.Close();
        }
        else
        {
            data = File .ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);

            if (worldData.chunkPrices.Count < world.childCount)
                UpdateData();
        }

    }
    private void UpdateData()
    {
        int missingData = world.childCount -worldData.chunkPrices.Count;

        for (int i = 0; i < missingData; i++)
        {
            int chunkIndex = world.childCount - missingData + i;
            int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
            worldData.chunkPrices.Add(chunkPrice);
        }
    }
     
    private void SaveWorld()
    {
        if(worldData.chunkPrices.Count != world.childCount)
            worldData= new WorldData();

        for(int i = 0;i < world.childCount; i++)
        {
            int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();

            if(worldData.chunkPrices.Count>i)
                worldData.chunkPrices[i] = chunkCurrentPrice;
            else
             worldData.chunkPrices.Add(chunkCurrentPrice);
        }

        string data = JsonUtility.ToJson(worldData,true);

        File.WriteAllText(dataPath, data);

        Debug.LogWarning("Data Saved");
    }

}
