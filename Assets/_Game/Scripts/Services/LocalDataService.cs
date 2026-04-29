using UnityEngine;

public class LocalDataService: ILocalDataService
{
    public int currentLevelId { get; set; }

    public LocalDataService()
    {
        currentLevelId = 0;
    }
}
