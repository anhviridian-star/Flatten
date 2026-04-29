#if UNITY_EDITOR
using UnityEngine;

public interface ILevelSaveService
{
    void CreateNewLevelData(LevelProperties newLevel);
}
#endif