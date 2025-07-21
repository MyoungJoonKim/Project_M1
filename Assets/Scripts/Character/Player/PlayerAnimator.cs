using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWalkStandard()
    {
        animator.SetBool("Walk_Standard", true);
    }
    public void StartRunStandard()
    {
        animator.SetBool("Run_Standard", true);
    }
    public void EndRunStandard()
    {
        animator.SetBool("Run_Standard", false);
    }
}
