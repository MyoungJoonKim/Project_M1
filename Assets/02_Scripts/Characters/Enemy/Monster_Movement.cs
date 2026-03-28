using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster_Movement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed = 7f;
    private Rigidbody2D rb;
    private Monster_Animator monster_Animator;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        monster_Animator = GetComponent<Monster_Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        MonsterMove();
    }

    void MonsterMove()
    {
        // 몬스터 추적 이동
        if (target == null)
            return;
        Vector2 dir = (target.position - transform.position).normalized;
        rb.velocity = dir * speed;

        float moveX = rb.velocity.x;
        Vector3 scale = transform.localScale;

        
        // 몬스터 좌우 반전
        if (moveX < 0)
            scale.x = Mathf.Abs(scale.x);
        else if (moveX > 0)
            scale.x = Mathf.Abs(scale.x);


        // 몬스터 이동 애니메이션
        if (rb.velocity != Vector2.zero)
            monster_Animator.SetMove(true);
        else
            monster_Animator.SetMove(false);
    }

    
    
}
