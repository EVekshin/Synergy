// BonusAnimation.cs
using UnityEngine;

public class BonusAnimation : MonoBehaviour
{
    private Animator animator;
    private static readonly int IsPlayerNear = Animator.StringToHash("IsPlayerNear");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(IsPlayerNear, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(IsPlayerNear, false);
        }
    }
}