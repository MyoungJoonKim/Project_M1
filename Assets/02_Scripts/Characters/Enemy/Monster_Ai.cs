using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster_Ai : MonoBehaviour
{
    private Monster monster;
    private Monster_Attack monster_Attack;
    private Monster_Animator monster_Animator;

    private Rigidbody2D rb;

    public MonsterState currentState = MonsterState.Idle;

    private void Start()
    {
        monster = GetComponent<Monster>();
        monster_Attack = GetComponent<Monster_Attack>();
        monster_Animator = GetComponent<Monster_Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (monster == null || monster.isDead)
            return;

        switch (currentState)
        {
            case MonsterState.Move:
                MoveToTarget();
                break;
        }
    }

    private void Update()
    {
        if (monster == null || monster.isDead)
            return;

        StateUpdate();
    }

    public void ChangeState(MonsterState newState)
    {
        if (currentState == newState) 
            return;

        currentState = newState;
        EnterState(newState);
    }

    private void StateUpdate()
    {
        if (monster.GetTarget() == null)
        {
            ChangeState(MonsterState.Idle);
            return;
        }

        MonsterData data = monster.GetMonsterData();
        if (data == null)
            return;

        float distance = Vector2.Distance(transform.position, monster.GetTarget().position);

        switch (currentState)
        {
            case MonsterState.Idle:
                if (distance > data.attackRange)
                    ChangeState(MonsterState.Move);
                break;

            //case MonsterState.Move:

            case MonsterState.Attack:
                if (distance > data.attackRange)
                {
                    ChangeState(MonsterState.Move);
                }
                else
                {
                    if (monster_Attack != null)
                        monster_Attack.TryAttack();
                }
                break;
        }
    }

    private void EnterState(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Idle:
                break;

            case MonsterState.Move:
                break;

            case MonsterState.Attack:
                if (monster_Attack != null)
                    monster_Attack.TryAttack();
                break;

            case MonsterState.Dead:
                StopMove();
                if (monster_Attack != null)
                    monster_Attack.StopAttack();
                if (monster_Animator != null)
                    monster_Animator.Dead();
                break;
        }
    }
    private void MoveToTarget()
    {
        Transform target = monster.GetTarget();
        if (target == null)
            return;

        Vector2 dir = (target.position - transform.position).normalized;

        float moveSpeed = monster.GetStat(StatType.MoveSpeed);
        rb.velocity = dir * moveSpeed;

        if (monster_Animator != null)
        {
            monster_Animator.SetMove(rb.velocity.sqrMagnitude > 0.01f);
            monster_Animator.SetFlip(dir.x);
        }
    }

    private void StopMove()
    {
        if (rb != null)
            rb.velocity = Vector2.zero;

        if (monster_Animator != null)
            monster_Animator.SetMove(false);
    }
    //void MonsterMove()
    //{
    //    // 몬스터 추적 이동
    //    if (target == null)
    //        return;
    //    Vector2 dir = (target.position - transform.position).normalized;
    //    rb.velocity = dir * speed;

    //    float moveX = rb.velocity.x;
    //    Vector3 scale = transform.localScale;

        
    //    // 몬스터 좌우 반전
    //    if (moveX < 0)
    //        scale.x = Mathf.Abs(scale.x);
    //    else if (moveX > 0)
    //        scale.x = Mathf.Abs(scale.x);


    //    // 몬스터 이동 애니메이션
    //    if (rb.velocity != Vector2.zero)
    //        monster_Animator.SetMove(true);
    //    else
    //        monster_Animator.SetMove(false);
    //}

    
    
}
