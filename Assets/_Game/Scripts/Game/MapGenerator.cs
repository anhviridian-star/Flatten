using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Properties")]
    [SerializeField] private LevelProperties levelProperties;
    private int layer = 0;
    private int height = 0;
    private int width = 0;

    [Header("Map Code")]
    private List<int> mapCode = new List<int>();

    [Header("Block Control")]
    private Dictionary<Vector3Int, Block> blockDict = new();
    private int fixedBlockCount = 0;
    private int blockCount = 0;

    [Header("Map Creation")]
    [SerializeField] private Block blockPrefab;
    [SerializeField] private Transform mapTf;

    [Header("Block Customization")]
    [SerializeField] private List<Sprite> blockSprs = new List<Sprite>();

    public async Task InitLevel(LevelProperties newLevelProperties)
    {
        levelProperties = newLevelProperties;
        foreach (KeyValuePair<Vector3Int, Block> pair in blockDict)
        {
            Destroy(pair.Value.gameObject);
        }

        blockDict.Clear();
        fixedBlockCount = 0;

        layer = levelProperties.layer;
        height = levelProperties.height;
        width = levelProperties.width;
        mapCode = levelProperties.mapCode.ToList();

        await GenerateMap();

        blockCount = fixedBlockCount;
    }

    private async Awaitable GenerateMap()
    {
        for (int k = 0; k < layer; k++)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int blockValue = GetBlockValue(new Vector3Int(j, i, k));
                    if (blockValue == 1)
                    {
                        Block block = Instantiate(blockPrefab, mapTf);
                        block.SetPosition((j - i) * Constant.MapWidthGap, 
                                        (j + i + k * 2) * Constant.MapHeightGap,
                                        Constant.LayerMultiplier * (i + 1) + (j + 1) - 10 * k);

                        block.ChangeBlockRend(blockSprs[k]);
                        blockDict.Add(new Vector3Int(j, i, k), block);
                        fixedBlockCount++;

                        await Awaitable.WaitForSecondsAsync(0.1f);
                        if (this == null) return;
                    }
                }
            }
        }
    }

    public void DestroyBlock(Vector3Int destroyPosition)
    {
        int destroyIndex = GetBlockIndex((Vector2Int)destroyPosition);
        blockDict[destroyPosition].gameObject.SetActive(false);

        mapCode[destroyIndex] -=  (int)Mathf.Pow(Constant.LayerMultiplier, destroyPosition.z);
        blockCount--;
    }

    public bool CheckWin()
    {
        return blockCount == 1;
    }

    public void Restart()
    {        
        mapCode.Clear();
        mapCode = levelProperties.mapCode.ToList();

        foreach (var item in blockDict)
        {
            item.Value.gameObject.SetActive(true);
        }

        blockCount = fixedBlockCount;
    }

    #region Helpers

    // Return Top Layer of a Position of a Position in 2 Dimension
    public int GetTopLayer(Vector2Int pos)
    {
        int count = 0;
        int currentIndexValue = mapCode[GetBlockIndex(pos)];
        while (currentIndexValue > Constant.LayerMultiplier)
        {
            count += 1;
            currentIndexValue = Mathf.FloorToInt(currentIndexValue / Constant.LayerMultiplier);
        }
        return count;
    }

    // Return Top Layer of a Position knowing Code Value
    public int GetTopLayer(int valueCode)
    {
        int count = 0;
        while (valueCode > Constant.LayerMultiplier)
        {
            count += 1;
            valueCode = Mathf.FloorToInt(valueCode / Constant.LayerMultiplier);
        }
        return count;
    }

    // Return Block Value (Type) in 3 Dimension
    private int GetBlockValue(Vector3Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height || pos.z < 0 || pos.z >= layer) return 0;

        return Mathf.FloorToInt(mapCode[GetBlockIndex((Vector2Int)pos)] / Mathf.Pow(Constant.LayerMultiplier, pos.z)) % Constant.LayerMultiplier;
    }

    // Return Whole Code Value of a 2 Dimension Position (including all the layers and value)
    public int GetBlockCodeValue(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height) return 0;

        return mapCode[GetBlockIndex(pos)];
    }

    // Return Block Index in Map Code List
    private int GetBlockIndex(Vector2Int pos)
    {
        return pos.x + pos.y * width;
    }
    #endregion
}
