using System;
using UnityEngine;

public class SaveDataService: MonoBehaviour, ISaveDataService
{
    [SerializeField] private SaveData currentData;
    public SaveData CurrentData => currentData;

    private const string DATA_SAVED = "DataSaved";
    private bool isLoaded = false;

    public void Init()
    {
        LoadData();
    }

    public void LoadData()
    {
        try
        {
            if (PlayerPrefs.HasKey(DATA_SAVED))
            {
                currentData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(DATA_SAVED));
            }
            else
            {
                currentData = new SaveData();
                PlayerPrefs.SetString(DATA_SAVED, JsonUtility.ToJson(currentData));
                PlayerPrefs.Save();
            }

            isLoaded = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Load Data Error: " + e);
        }
    }

    public void SaveData()
    {
        try
        {
            if (!isLoaded) return;

            PlayerPrefs.SetString(DATA_SAVED, JsonUtility.ToJson(currentData));
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogError("Save Data Error: " + e);
        }
    }

    public SaveData GetData()
    {
        return CurrentData;
    }
}
