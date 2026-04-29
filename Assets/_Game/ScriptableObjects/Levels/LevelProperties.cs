using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelProperties", menuName = "Scriptable Objects/LevelProperties")]
public class LevelProperties : ScriptableObject
{
    public int layer;
    public int height;
    public int width;
    public List<int> mapCode;

    public Vector3Int playerPosition;
    public Vector3 cameraPosition;
    public float cameraSize;
}