#if UNITY_EDITOR
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockCreator : MonoBehaviour, IPointerDownHandler
{
    private int layer = 0;
    [SerializeField] private TMP_Text layerText;
    private LevelCreator levelCreator;
    public Vector2Int mapPos { get; private set; }
    [SerializeField] private GameObject greenFrame;
    
    public void Init(LevelCreator newLevelCreator, Vector2Int newMapPos)
    {
        levelCreator = newLevelCreator;
        mapPos = newMapPos;

        SetLayerText(layer);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        // Add 1 Layer (left Button)
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            AddLayer();
        }

        // Remove 1 Layer (left Button)
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RemoveLayer();
        }

        // Choose Player Start Position
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            SetPlayerStartBlock();
        }
    }

    private void SetPlayerStartBlock()
    {
        levelCreator.SetPlayerStartBlock(this);
    }

    public void SetAsPlayerPos()
    {
        greenFrame.SetActive(true);
    }

    public void UnsetAsPlayerPos()
    {
        greenFrame.SetActive(false);
    }

    private void AddLayer()
    {
        if (layer >= 3) return;

        layer++;
        SetLayerText(layer);

        levelCreator.AddLayer(mapPos);
    }

    private void RemoveLayer()
    {
        if (layer == 0) return;

        layer--;
        SetLayerText(layer);

        levelCreator.RemoveLayer(mapPos);
    }

    public void SetLayerText(int newLayer)
    {
        if (newLayer == 0) layerText.text = "";
        else layerText.text = newLayer.ToString();
    }
}
#endif