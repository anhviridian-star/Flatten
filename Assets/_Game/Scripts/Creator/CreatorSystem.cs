#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

public class CreatorSystem : MonoBehaviour
{
    [Header("Creation UI")]
    [SerializeField] private GameObject levelCreatorGo;
    [SerializeField] private GameObject playtestGo;

    [SerializeField] private Button exitPlayTest;
    [SerializeField] private Button submitLevel;

    [Header("Dependency")]
    [SerializeField] private GameManager gameManager;

    [Header("Level Save")]
    private ILevelSaveService levelSaveService;
    private LevelProperties currentLevelData;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;

        exitPlayTest.onClick.RemoveAllListeners();
        exitPlayTest.onClick.AddListener(ExitPlayTest);

        submitLevel.onClick.RemoveAllListeners();
        submitLevel.onClick.AddListener(SaveNewLevel);

        ServiceAccess.Register<ILevelSaveService>(new LevelSaveService());
        levelSaveService = ServiceAccess.Get<ILevelSaveService>();
    }

    private void Start()
    {
        levelCreatorGo.SetActive(true);
        playtestGo.SetActive(false);
    }

    public void OpenPlayTest(LevelProperties levelProperties)
    {
        if (levelProperties == null) return;

        levelCreatorGo.SetActive(false);
        playtestGo.SetActive(true);

        currentLevelData = levelProperties;
        gameManager.SetLevelProperties(currentLevelData);
    }

    private void ExitPlayTest()
    {
        playtestGo.SetActive(false);
        levelCreatorGo.SetActive(true);
    }

    private void SaveNewLevel()
    {
        currentLevelData.cameraPosition = cam.transform.position;
        currentLevelData.cameraSize = cam.orthographicSize;

        levelSaveService.CreateNewLevelData(currentLevelData);
    }
}
#endif