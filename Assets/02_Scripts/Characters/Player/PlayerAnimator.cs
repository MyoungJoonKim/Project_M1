using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;

    public void SetMove(bool state)
    {
        animator.SetBool("isMoving", state);
    }

    public void SetDead(bool state)
    {
        animator.SetBool("isDead", state);
        StartCoroutine(StopAnimator());
    }

    public void Hit()
    {
        if (animator == null)
            return;

        animator.SetTrigger("isHit");
    }

    IEnumerator StopAnimator()
    {
        yield return new WaitForSeconds(1f);
        animator.speed = 0f;
    }
}
