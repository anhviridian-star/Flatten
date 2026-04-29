using DG.Tweening;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    private Camera cam;

    public void AwakeSetup()
    {
        cam = Camera.main;
    }

    public void Init(LevelProperties levelProperties)
    {
        cam.transform.position = levelProperties.cameraPosition;
        cam.orthographicSize = levelProperties.cameraSize;
    }

    public void Win(Transform playerTransform)
    {
        cam.transform.SetParent(playerTransform);
        cam.transform.DOLocalMove(new Vector3(0, 0, -10), 0.5f);
        DOVirtual.Float(cam.orthographicSize, 3.5f, 0.75f, value =>
        {
            cam.orthographicSize = value;
        });
    }

    public void Restart(LevelProperties levelProperties)
    {
        cam.transform.SetParent(null);
        cam.transform.position = levelProperties.cameraPosition;
        cam.orthographicSize = levelProperties.cameraSize;
    }
}
