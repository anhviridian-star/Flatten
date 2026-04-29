using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreator : MonoBehaviour
{
    [Header("Creator Map Properties")]
    [SerializeField] private int height = 10;
    [SerializeField] private int width = 10;
    private WaitForEndOfFrame ieWait = new WaitForEndOfFrame();

    [Header("Map Code")]
    private List<int> mapCode = new();

    [Header("Block Control")]
    [SerializeField] private BlockCreator blockPrefab;
    private BlockCreator playerStartBlock;
    [SerializeField] private Transform blockHolder;

    [Header("Changing State Properties")]
    [SerializeField] private Transform cameraTf;
    [SerializeField] private Button testBtn;

    [SerializeField] private CreatorSystem creatorSystem;

    private void OnEnable()
    {
        // Set Camera
        cameraTf.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    private void Start()
    {
        // Set Button
        testBtn.onClick.RemoveAllListeners();
        testBtn.onClick.AddListener(TestNewLevel);

        StartCoroutine(IEGenerateGridMap());
    }

    IEnumerator IEGenerateGridMap()
    {
        yield return ieWait;

        // Create Grid
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                BlockCreator newBlock = Instantiate(blockPrefab, blockHolder);
                newBlock.transform.position = new Vector2(j, i);
                newBlock.Init(this, new Vector2Int(j, i));

                mapCode.Add(0);

                yield return ieWait;
            }
        }

        //Debug.Log(mapCode.Count);
    }

    public void AddLayer(Vector2Int mapPos)
    {
        int blockIndex = GetIndexInMapCode(mapPos);
        mapCode[blockIndex] += (int)Mathf.Pow(Constant.LayerMultiplier, GetTopLayer(mapCode[blockIndex]) + 1);
    }

    public void RemoveLayer(Vector2Int mapPos)
    {
        int blockIndex = GetIndexInMapCode(mapPos);
        mapCode[blockIndex] -= (int)Mathf.Pow(Constant.LayerMultiplier, GetTopLayer(mapCode[blockIndex]));
    }

    public void SetPlayerStartBlock(BlockCreator newBlock)
    {
        if (playerStartBlock != newBlock)
        {
            playerStartBlock?.UnsetAsPlayerPos();
            playerStartBlock = newBlock;
            playerStartBlock.SetAsPlayerPos();
        }
    }

    private LevelProperties CreateNewLevel()
    {
        int minWidthPos = 1000;
        int minHeightPos = 1000;
        int maxWidthPos = -1;
        int maxHeightPos = -1;
        int maxLayer = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int mapCodeValue = mapCode[GetIndexInMapCode(new Vector2Int(j, i))];
                if (mapCodeValue > 0)
                {
                    if (j < minWidthPos) minWidthPos = j;
                    if (i < minHeightPos) minHeightPos = i;
                    if (j > maxWidthPos) maxWidthPos = j;
                    if (i > maxHeightPos) maxHeightPos = i;

                    int topLayer = GetTopLayer(mapCodeValue) + 1;
                    if (topLayer > maxLayer) maxLayer = topLayer;
                }
            }
        }

        int levelWidth = maxWidthPos - minWidthPos + 1;
        int levelHeight = maxHeightPos - minHeightPos + 1;
        List<int> levelMapCode = new List<int>();

        for (int i = 0; i < levelHeight; i++)
        {
            for (int j = 0; j < levelWidth; j++)
            {
                levelMapCode.Add(mapCode[GetIndexInMapCode(new Vector2Int(j + minWidthPos, i + minHeightPos))]);
            }
        }

        print("Level Map Code: " + GetMapCodeStr(levelMapCode));

        if (playerStartBlock == null)
        {
            Debug.LogWarning("Please choose a Start Block");
            return null;
        }

        Vector2Int playerStartPos2D = playerStartBlock.mapPos;
        Vector3Int playerStartPos = new Vector3Int(
            playerStartPos2D.x - minWidthPos, 
            playerStartPos2D.y - minHeightPos,
            GetTopLayer(mapCode[GetIndexInMapCode(playerStartPos2D)])
        );

        print("Player Start Pos: " + playerStartPos);

        LevelProperties newLevel = ScriptableObject.CreateInstance<LevelProperties>();
        newLevel.layer = maxLayer;
        newLevel.width = levelWidth;
        newLevel.height = levelHeight;
        newLevel.mapCode = levelMapCode;
        newLevel.playerPosition = playerStartPos;

        // Set Default Value
        newLevel.cameraPosition = new Vector3(0, 0, -10);
        newLevel.cameraSize = 5;

        return newLevel;
    }

    private void TestNewLevel()
    {
        creatorSystem.OpenPlayTest(CreateNewLevel());
    }

    #region Helpers
    public int GetTopLayer(int valueCode)
    {
        if (valueCode == 0) return -1;

        int count = 0;
        while (valueCode > Constant.LayerMultiplier)
        {
            count += 1;
            valueCode = Mathf.FloorToInt(valueCode / Constant.LayerMultiplier);
        }
        return count;
    }

    private int GetIndexInMapCode(Vector2Int vec)
    {
        return vec.x + vec.y * width; 
    }

    private string GetMapCodeStr(List<int> checkMapCode)
    {
        string result = "";
        for (int i = 0; i < checkMapCode.Count; i++)
        {
            result += checkMapCode[i].ToString() + " ";
        }

        return result;
    }
    #endregion
}
