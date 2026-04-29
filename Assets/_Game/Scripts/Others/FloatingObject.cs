using UnityEngine;
using DG.Tweening;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private RectTransform floatRectTf;
    [SerializeField] private float floatDistance;
    [SerializeField] private bool xAxis;

    private float targetPosAxis;
    private bool activated = false;

    private void OnEnable()
    {
        if (activated) return;
        activated = true;

        if (xAxis)
        {
            targetPosAxis = floatRectTf.anchoredPosition.x + floatDistance;
            floatRectTf.DOAnchorPosX(targetPosAxis, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            targetPosAxis = floatRectTf.anchoredPosition.y + floatDistance;
            floatRectTf.DOAnchorPosY(targetPosAxis, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
