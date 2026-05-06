using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private Player player;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameUIController gameUIController;
    [SerializeField] private GameCameraController gameCameraController;
    private BlockController blockController;

    [Header("Ingame Properties")]
    [SerializeField] private LevelProperties currentLevelProperties;
    private bool playable;

    [Header("Optional")]
    [SerializeField] private bool isCreating = false;
    [SerializeField] private bool isTestInGame = false;

    [Header("Services")]
    private ISoundService soundService;
    private ILocalDataService localDataService;
    private ILevelService levelService;
    private ISaveDataService saveDataService;

    private void Start()
    {
        soundService = ServiceAccess.Get<ISoundService>();
        localDataService = ServiceAccess.Get<ILocalDataService>();
        levelService = ServiceAccess.Get<ILevelService>();
        saveDataService = ServiceAccess.Get<ISaveDataService>();

        soundService.PlaySoundBackground(BACKGROUND_SOUND.GAME_BG);

        playable = false;

        blockController = new BlockController();

        player.AwakeSetup(this, mapGenerator);
        gameUIController.AwakeSetup(this);
        gameCameraController.AwakeSetup();

        player.gameObject.SetActive(false);

        if (isCreating) return;

        int currentLevelId = localDataService.currentLevelId;
        currentLevelProperties = levelService.GetLevel(currentLevelId);

        InitGame();
    }

    private async void InitGame()
    {
        gameCameraController.Init(currentLevelProperties);

        await mapGenerator.InitLevel(currentLevelProperties);

        player.gameObject.SetActive(true);
        player.SetInitMapPositon(currentLevelProperties.playerPosition);

        await gameUIController.Init();

        playable = true;
    }

    private void Update()
    {
        if (!playable) return;

        if (Keyboard.current.wKey.wasPressedThisFrame) player.MoveUp();
        else if (Keyboard.current.sKey.wasPressedThisFrame) player.MoveDown();
        else if (Keyboard.current.dKey.wasPressedThisFrame) player.MoveRight();
        else if (Keyboard.current.aKey.wasPressedThisFrame) player.MoveLeft();
    }

    public void ManualPlayerInput(MoveDirection moveDirection)
    {
        if (!playable) return;

        switch (moveDirection)
        {
            case MoveDirection.Left:
                player.MoveLeft(); break;
            case MoveDirection.Right:
                player.MoveRight(); break;
            case MoveDirection.Up:
                player.MoveUp(); break;
            case MoveDirection.Down:
                player.MoveDown(); break;
        }
    }

    public void PlayerMoveToNewBlock(Vector3Int move, Vector3 moveScale, Vector3Int currentPosition)
    {
        soundService.PlaySoundEffect(EFFECT_SOUND.PLAYER_MOVE);

        mapGenerator.DestroyBlock(currentPosition);
        player.MoveToNewBlock(move, moveScale);
    }

    public void HandleTransparentColliders(Collider2D[] overlapColliders)
    {
        blockController.SetTransparentBlocksByColliders(overlapColliders);
    }

    public void CheckWinGame()
    {
        if (mapGenerator.CheckWin() && !isCreating)
        {
            soundService.PlaySoundEffect(EFFECT_SOUND.WIN_GAME);
            player.Win();
            gameCameraController.Win(player.transform);

            gameUIController.Win();

            // Services
            saveDataService.GetData().currentLevel++;
        }
    }

    public async void RestartLevel()
    {
        soundService.PlaySoundEffect(EFFECT_SOUND.BTN_CLICK);

        DOTween.KillAll();

        if (isTestInGame) await mapGenerator.InitLevel(currentLevelProperties);
        else mapGenerator.Restart();

        player.SetInitMapPositon(currentLevelProperties.playerPosition);
        gameCameraController.Restart(currentLevelProperties);
    }

    public void ReturnHome()
    {
        soundService.PlaySoundEffect(EFFECT_SOUND.BTN_CLICK);

        DOTween.KillAll();

        SceneManager.LoadScene("Home");
    }

    #region Creation
    public void SetLevelProperties(LevelProperties newLevelProperties)
    {
        currentLevelProperties = newLevelProperties;
        InitGame();
    }
    #endregion
}

[Serializable]
public enum MoveDirection
{
    Up,
    Down,
    Left,
    Right
}
