#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelSaveService: ILevelSaveService
{
    public void CreateNewLevelData(LevelProperties newLevel)
    {
        string folderPath = "Assets/_Game/ScriptableObjects";
        string fileName = "NewLevelData.asset";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        string fullPath = Path.Combine(folderPath, fileName);
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

        AssetDatabase.CreateAsset(newLevel, uniquePath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newLevel;
        Debug.Log("Successfully Created");
    }
}
#endif