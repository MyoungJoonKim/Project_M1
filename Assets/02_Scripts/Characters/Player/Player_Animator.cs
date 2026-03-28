using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animator : MonoBehaviour
{
    public Animator animator;

    public void SetMove(bool state)
    {
        animator.SetBool("isMoving", state);
    }
}
