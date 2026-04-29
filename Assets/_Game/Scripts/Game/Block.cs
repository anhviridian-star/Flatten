using TMPro;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    public SpriteRenderer blockRend;
    [SerializeField] private Transform thisTf;

    public void ChangeBlockRend(Sprite newSpr)
    {
        blockRend.sprite = newSpr;
    }

    public void SetPosition(float x, float y, float z)
    {
        thisTf.position = new Vector3(x, y + 0.2f, z);
        thisTf.DOMoveY(y, 0.1f);

        DOVirtual.Float(0.5f, 1f, 0.1f, value =>
        {
            blockRend.color = new Color(1, 1, 1, value);
        });

    }

    public void SetTransparent(bool transparent)
    {
        if (transparent) blockRend.color = new Color(1, 1, 1, 0.5f);
        else blockRend.color = new Color(1, 1, 1, 1.0f);
    }
}
