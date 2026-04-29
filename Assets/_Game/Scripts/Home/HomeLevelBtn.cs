using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HomeLevelBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Button btn;
    [SerializeField] private GameObject lockGo;
    [SerializeField] private TMP_Text levelText;

    private int levelBtnId;
    private HomeManager homeManager;
    private bool isLocked;
    private RectTransform thisRectTf;

    public void Init(HomeManager newHomeManager, int newLevelBtnId)
    {
        thisRectTf = gameObject.GetComponent<RectTransform>();

        homeManager = newHomeManager;

        levelBtnId = newLevelBtnId;
        levelText.text = "Level " + (levelBtnId + 1);

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => homeManager.PlayLevel(levelBtnId));

        CheckAvailability();
    }

    private void CheckAvailability()
    {
        if (levelBtnId <= ServiceAccess.Get<ISaveDataService>().GetData().currentLevel)
        {
            Unlock();
        }
        else
        {
            Lock();
        }
    }

    private void Unlock()
    {
        isLocked = false;

        btn.interactable = true;
        lockGo.SetActive(false);
    }

    private void Lock()
    {
        isLocked = true;

        btn.interactable = false;
        lockGo.SetActive(true);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        this.transform.DOKill();
        thisRectTf.DOKill();
        lockGo.transform.DOKill();

        if (!isLocked)
        {
            this.transform.DOScale(1.1f, 0.5f);
            this.transform.DORotate(new Vector3(0, 0, Random.Range(5f, 10f) * (Random.value > 0.5f ? 1f : -1f)), 0.5f);
        }
        else
        {
            lockGo.transform.DOShakeRotation(0.5f, 90);
            lockGo.transform.DOShakeScale(0.5f, 0.7f);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        this.transform.DOKill();
        thisRectTf.DOKill();

        lockGo.transform.DOKill();

        if (!isLocked)
        {
            this.transform.DOScale(1f, 0.5f);
            transform.DORotate(Vector3.zero, 0.5f);
        }
        else
        {
            lockGo.transform.eulerAngles = Vector3.zero;
            lockGo.transform.localScale = Vector3.one;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        this.transform.DOKill();
        thisRectTf.DOKill();

        this.transform.DOScale(1f, 0.5f);
    }

    private void OnDestroy()
    {
        this.transform.DOKill();
        thisRectTf.DOKill();
    }
}
