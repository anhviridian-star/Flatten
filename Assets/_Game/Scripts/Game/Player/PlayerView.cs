using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerRend;
    public Transform playerTf { get; private set; }

    public void Init(Transform newPlayerTf)
    {
        playerTf = newPlayerTf;
    }

    public void PlayAnimation(PlayerAnimationType animationType, bool flip)
    {
        playerRend.flipX = flip;
        animator.Play(Constant.PlayerAnimationNames[(int)animationType]);
    }

    public void PauseAnimation()
    {
        animator.speed = 0;
    }

    public void ResumeAnimation()
    {
        animator.speed = 1;
    }

    public void FlipRend()
    {
        playerRend.flipX = !playerRend.flipX;
    }

    public async Task InitPosition(Vector3 newPosition)
    {
        playerTf.position = new Vector3(newPosition.x, newPosition.y + 0.2f, newPosition.z);
        playerTf.DOMoveY(newPosition.y, 0.1f);

        Tween initTween = DOVirtual.Float(0.5f, 1f, 0.1f, value =>
        {
            playerRend.color = new Color(1, 1, 1, value);
        });

        await initTween.AsyncWaitForCompletion();
    }

    public void SetPosition(Vector3 newPosition)
    {
        playerTf.position = newPosition;
    }

    public async Task JumpToPosition(Vector3 newPosition)
    {
        Tween jumpTween = playerTf.DOJump(newPosition, 0.5f, 1, 0.5f);
        await jumpTween.AsyncWaitForCompletion();
    }
}
