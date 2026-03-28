using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Animator : MonoBehaviour
{
    public Animator animator;

    public void SetMove(bool state)
    {
        animator.SetBool("isMoving", state);
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
    }
}
