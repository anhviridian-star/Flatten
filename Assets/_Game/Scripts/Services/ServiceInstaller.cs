using System.Collections.Generic;
using UnityEngine;

public class ServiceInstaller : MonoBehaviour
{
    private static ServiceInstaller _instance;

    [SerializeField] private LevelService levelService;
    private LocalDataService localDataService;
    [SerializeField] SaveDataService saveDataService;
    [SerializeField] private SoundService soundService;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            Init();
        }
    }

    public void Init()
    {
        ServiceAccess.Register<ILevelService>(levelService);
        
        localDataService = new LocalDataService();
        ServiceAccess.Register<ILocalDataService>(localDataService);

        saveDataService.LoadData();
        ServiceAccess.Register<ISaveDataService>(saveDataService);

        soundService.Init();
        ServiceAccess.Register<ISoundService>(soundService);

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (_instance == this) saveDataService.SaveData();
    }
}
