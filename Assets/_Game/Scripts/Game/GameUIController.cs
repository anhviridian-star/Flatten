using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class GameUIController : MonoBehaviour
{
    [Header("Common Buttons")]
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button homeBtn;

    [SerializeField] private Button homeWinBtn;

    [Header("Move Buttons")]
    [SerializeField] private Button moveUpBtn;
    [SerializeField] private Button moveDownBtn;
    [SerializeField] private Button moveLeftBtn;
    [SerializeField] private Button moveRightBtn;

    [Header("Effects")]
    [SerializeField] private RectTransform moveBtnHolderRectTf;
    [SerializeField] private RectTransform homeBtnRectTf;
    [SerializeField] private RectTransform restartBtnRectTf;
    [SerializeField] private RectTransform homeWinBtnRectTf;
    [SerializeField] private RectTransform winTitleRectTf;

    [SerializeField] private FloatingObject winTitleFloatObj;
    [SerializeField] private EffectButton homeEffectBtn;
    [SerializeField] private EffectButton restartEffectBtn;
    [SerializeField] private EffectButton homeWinEffectBtn;

    private GameManager gameManager;

    public void AwakeSetup(GameManager newGameManager)
    {
        gameManager = newGameManager;

        restartBtn.onClick.RemoveAllListeners();
        restartBtn.onClick.AddListener(gameManager.RestartLevel);

        homeBtn.onClick.RemoveAllListeners();
        homeBtn.onClick.AddListener(gameManager.ReturnHome);

        homeWinBtn.onClick.RemoveAllListeners();
        homeWinBtn.onClick.AddListener(gameManager.ReturnHome);

        moveUpBtn.onClick.RemoveAllListeners();
        moveDownBtn.onClick.RemoveAllListeners();
        moveLeftBtn.onClick.RemoveAllListeners();
        moveRightBtn.onClick.RemoveAllListeners();

        moveUpBtn.onClick.AddListener(() => gameManager.ManualPlayerInput(MoveDirection.Up));
        moveDownBtn.onClick.AddListener(() => gameManager.ManualPlayerInput(MoveDirection.Down));
        moveLeftBtn.onClick.AddListener(() => gameManager.ManualPlayerInput(MoveDirection.Left));
        moveRightBtn.onClick.AddListener(() => gameManager.ManualPlayerInput(MoveDirection.Right));
    }

    public async Task Init()
    {
        homeEffectBtn.enabled = false;
        restartEffectBtn.enabled = false;

        homeBtnRectTf.DOAnchorPosX(45, 0.5f).SetEase(Ease.OutSine);
        restartBtnRectTf.DOAnchorPosX(-45f, 0.5f).SetEase(Ease.OutSine);
        Tween moveTween = moveBtnHolderRectTf.DOAnchorPosY(540f, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            homeEffectBtn.enabled = true;
            restartEffectBtn.enabled = true;
        });

        await moveTween.AsyncWaitForCompletion();
    }

    public void Win()
    {
        homeBtn.enabled = false;
        restartBtn.enabled = false;
        homeEffectBtn.enabled = false;
        restartEffectBtn.enabled = false;
        homeWinEffectBtn.enabled = false;
        homeWinBtn.enabled = false;

        moveUpBtn.enabled = false;
        moveDownBtn.enabled = false;
        moveLeftBtn.enabled = false;
        moveRightBtn.enabled = false;

        homeBtnRectTf.DOAnchorPosX(-150f, 0.5f).SetEase(Ease.InBack);
        restartBtnRectTf.DOAnchorPosX(150f, 0.5f).SetEase(Ease.InBack);
        moveBtnHolderRectTf.DOAnchorPosY(240, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            homeWinBtnRectTf.DOAnchorPosY(220f, 0.5f).SetEase(Ease.OutBack);

            winTitleRectTf.DOAnchorPosY(-240f, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                winTitleFloatObj.enabled = true;

                homeWinEffectBtn.enabled = true;
                homeWinBtn.enabled = true;
            });

        });
    }
}
