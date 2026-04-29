using UnityEngine;

public interface ISaveDataService
{
    SaveData CurrentData { get; }
    public void LoadData();
    public void SaveData();
    public SaveData GetData()
    {
        return null;
    }
}

[System.Serializable]
public class SaveData
{
    public int currentLevel;

    public SaveData()
    {
        currentLevel = 0;
    }
}
