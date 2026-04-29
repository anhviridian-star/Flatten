using System.Collections;
using UnityEngine;

public class HomeCharacter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer charRend;
    [SerializeField] private Animator charAnimator;
    [SerializeField] private PlayerAnimationType playerAnimationType;

    private string[] animationNames =
{
        "GetUp",
        "Idle",
        "Idle_2",
        "Run",
        "Run_2",
        "Victory",
        "Victory_2",
        "Run_3",
        "Run_4",
    };

    private void Start()
    {
        StartCoroutine(IEWaitForAnim());
    }

    IEnumerator IEWaitForAnim()
    {
        yield return new WaitForSeconds(1.5f);
        charAnimator.Play(animationNames[(int)playerAnimationType]);
    }

    public void FlipRend()
    {
        charRend.flipX = !charRend.flipX;
    }
}
