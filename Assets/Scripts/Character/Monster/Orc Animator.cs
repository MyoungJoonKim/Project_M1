using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class OrcAnimator : MonoBehaviour
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

    public void SetMove(float move)
    {
        animator.SetFloat("Move", move);
    }

    public void SetAttack(float attack)
    {
        animator.SetFloat("Attack", attack);
    }

}
