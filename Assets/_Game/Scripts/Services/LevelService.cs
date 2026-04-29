using UnityEngine;
using System.Collections.Generic;

public class LevelService : MonoBehaviour, ILevelService
{
    [SerializeField] private List<LevelProperties> allLevelProperties = new();

    public LevelProperties GetLevel(int levelId)
    {
        return allLevelProperties[levelId];
    }
}
