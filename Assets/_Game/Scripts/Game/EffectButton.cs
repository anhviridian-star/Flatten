using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class EffectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private bool allowScaleEffect;
    [SerializeField] private bool allowRotateEffect;
    private Vector3 originalScale;
    private RectTransform rectTransfom;

    private void Start()
    {
        originalScale = transform.localScale;
        rectTransfom = this.GetComponent<RectTransform>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        this.transform.DOKill();
        rectTransfom.DOKill();

        if (allowScaleEffect) this.transform.DOScale(originalScale * 1.1f, 0.5f).SetLink(gameObject);
        if (allowRotateEffect) this.transform.DORotate(new Vector3(0, 0, Random.Range(5f, 10f) * (Random.value > 0.5f ? 1f : -1f)), 0.5f).SetLink(gameObject);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        this.transform.DOKill();
        rectTransfom.DOKill();

        if (allowScaleEffect) this.transform.DOScale(originalScale, 0.5f).SetLink(gameObject);
        if (allowRotateEffect) transform.DORotate(Vector3.zero, 0.5f).SetLink(gameObject);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        this.transform.DOKill();
        rectTransfom.DOKill();

        if (allowScaleEffect) this.transform.DOScale(originalScale, 0.5f).SetLink(gameObject);
    }

    private void OnDestroy()
    {
        this.transform.DOKill();
        rectTransfom.DOKill();
    }
}
