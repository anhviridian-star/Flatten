using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.Properties;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private List<HomeLevelBtn> allLevelBtns = new();

    [Header("Services")]
    private ISoundService soundService;
    private ILocalDataService localDataService;

    private void Start()
    {
        soundService = ServiceAccess.Get<ISoundService>();
        localDataService = ServiceAccess.Get<ILocalDataService>();

        soundService.PlaySoundBackground(BACKGROUND_SOUND.HOME_BG);

        for (int i = 0; i < allLevelBtns.Count; i++)
        {
            allLevelBtns[i].Init(this, i);
        }
    }

    public void PlayLevel(int id)
    {
        soundService.PlaySoundEffect(EFFECT_SOUND.BTN_CLICK);
        localDataService.currentLevelId = id;

        DOTween.KillAll();
        SceneManager.LoadScene("Game");
    }
}
