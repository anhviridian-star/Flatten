using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerView playerView;
    private PlayerLogic playerLogic;

    private GameManager gameManager;
    private MapGenerator mapGenerator;

    [Header("Temporaly Variables")]
    private int newBlockCode = 0;

    [Header("Overlapped Blocks")]
    [SerializeField] private Transform botLeftTf;
    [SerializeField] private Transform topRightTf;
    [SerializeField] private LayerMask blockLayerMask;

    [Header("Handle Overload Input")]
    private bool isJumping = false;
    private Queue<System.Action> moveActions = new();

    public void AwakeSetup(GameManager newGameManager, MapGenerator newMapGenerator)
    {
        gameManager = newGameManager;
        mapGenerator = newMapGenerator;

        playerView.Init(this.transform);
        playerLogic = new PlayerLogic();
    }

    public async void SetInitMapPositon(Vector3Int initMapPosition)
    {
        this.transform.DOKill();

        isJumping = false;
        moveActions.Clear();

        playerLogic.SetMapPosition(initMapPosition);

        playerView.PlayAnimation(PlayerAnimationType.GET_UP, false);
        playerView.PauseAnimation();
        await playerView.InitPosition(playerLogic.CalculateCurrentPosition(1));
        playerView.ResumeAnimation();

        CheckTransparentColliders();
    }

    public void MoveUp()
    {
        if (isJumping)
        {
            moveActions.Enqueue(MoveUp);
            return;
        }

        newBlockCode = mapGenerator.GetBlockCodeValue((Vector2Int)playerLogic.currentMapPosition + Vector2Int.up);
        if (newBlockCode > 0)
        {
            playerView.PlayAnimation(PlayerAnimationType.RUN_2, false);
            gameManager.PlayerMoveToNewBlock(Vector3Int.up, new Vector3(-1, 1, 1), playerLogic.currentMapPosition);
        }
    }

    public void MoveDown()
    {
        if (isJumping)
        {
            moveActions.Enqueue(MoveDown);
            return;
        }

        newBlockCode = mapGenerator.GetBlockCodeValue((Vector2Int)playerLogic.currentMapPosition + Vector2Int.down);
        if (newBlockCode > 0)
        {
            playerView.PlayAnimation(PlayerAnimationType.RUN, true);
            gameManager.PlayerMoveToNewBlock(Vector3Int.down, new Vector3(1, -1, 1), playerLogic.currentMapPosition);
        }
    }

    public void MoveLeft()
    {
        if (isJumping)
        {
            moveActions.Enqueue(MoveLeft);
            return;
        }

        newBlockCode = mapGenerator.GetBlockCodeValue((Vector2Int)playerLogic.currentMapPosition + Vector2Int.left);
        if (newBlockCode > 0)
        {
            playerView.PlayAnimation(PlayerAnimationType.RUN, false);
            gameManager.PlayerMoveToNewBlock(Vector3Int.left, new Vector3(-1, -1, 1), playerLogic.currentMapPosition);
        }
    }

    public void MoveRight()
    {
        if (isJumping)
        {
            moveActions.Enqueue(MoveRight);
            return;
        }

        newBlockCode = mapGenerator.GetBlockCodeValue((Vector2Int)playerLogic.currentMapPosition + Vector2Int.right);
        if (newBlockCode > 0)
        {
            playerView.PlayAnimation(PlayerAnimationType.RUN_2, true);
            gameManager.PlayerMoveToNewBlock(Vector3Int.right, new Vector3(1, 1, 1), playerLogic.currentMapPosition);
        }
    }

    public async void MoveToNewBlock(Vector3Int move, Vector3 moveScale)
    {
        int newBlockTopLayer = mapGenerator.GetTopLayer(newBlockCode);
        playerLogic.MoveMapPosition(move, newBlockTopLayer);

        Vector3 newPosition = playerLogic.CalculateCurrentPosition(1);
        Vector3 oldPosition = playerView.playerTf.position;

        playerView.SetPosition(newPosition);
        CheckTransparentColliders();

        newPosition = playerLogic.CalculateCurrentPosition(2);
        playerView.SetPosition(
            new Vector3(oldPosition.x,
                        oldPosition.y, 
                        newPosition.z)
            );

        isJumping = true;
        await playerView.JumpToPosition(newPosition);

        if (gameObject == null) return;

        playerView.SetPosition(playerLogic.CalculateCurrentPosition(1));
        //CheckTransparentColliders();
        gameManager.CheckWinGame();

        isJumping = false;
        if (moveActions.Count > 0) moveActions.Dequeue().Invoke();
    }

    public void Win()
    {
        moveActions.Clear();

        playerView.PlayAnimation(PlayerAnimationType.VICTORY, false);
    }

    private void CheckTransparentColliders()
    {
        Collider2D[] overlapBlockColliders = Physics2D.OverlapAreaAll(topRightTf.position, botLeftTf.position, layerMask: blockLayerMask, minDepth: 0f, maxDepth: playerView.playerTf.position.z);
        gameManager.HandleTransparentColliders(overlapBlockColliders);
    }
}
