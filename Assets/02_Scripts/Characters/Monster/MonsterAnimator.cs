using UnityEngine;

public class MonsterAnimator : MonoBehaviour
{
    [SerializeField] private Transform scaleFlip;
    public Animator animator;


    public void SetMove(bool state)
    {
        if (animator == null)
            return;

        animator.SetBool("isMoving", state);
    }

    public void Attack()
    {
        if (animator == null)
            return;

        animator.SetTrigger("attack");
    }

    public void Hit()
    {
        if (animator == null)
            return;

        animator.SetTrigger("hit");
    }

    public void Dead()
    {
        if (animator == null)
            return;

        animator.SetTrigger("Dead");
    }

    public void SetFlip(float moveX)
    {
        if (scaleFlip == null)
            return;

        if (Mathf.Abs(moveX) < 0.01f)
            return;

        Vector3 scale = scaleFlip.localScale;

        if (moveX > 0)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        scaleFlip.localScale = scale;
    }


}
