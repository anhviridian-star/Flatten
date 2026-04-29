using UnityEngine;

public class PlayerLogic
{
    public Vector3Int currentMapPosition { get; private set; }

    public void SetMapPosition(Vector3Int newMapPosition)
    {
        currentMapPosition = newMapPosition;
    }

    public Vector3 CalculateCurrentPosition(int uplevel)
    {
        return new Vector3(Constant.MapWidthGap * (currentMapPosition.x - currentMapPosition.y),
                        Constant.MapHeightGap * (currentMapPosition.x + currentMapPosition.y + 2 * currentMapPosition.z) + 0.8f,
                        Constant.LayerMultiplier * (currentMapPosition.y + 1) + (currentMapPosition.x + 1) - 10 * (currentMapPosition.z + uplevel));
                        //(currentMapPosition.x + currentMapPosition.y) / Mathf.Pow(Constant.LayerMultiplier, currentMapPosition.z + 1));
    }

    public void MoveMapPosition(Vector3Int move, int newTopLayer)
    {
        currentMapPosition += move;
        int zGap = newTopLayer - currentMapPosition.z;
        currentMapPosition += Vector3Int.forward * zGap;
    }
}
