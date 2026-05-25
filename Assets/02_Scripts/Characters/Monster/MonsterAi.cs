using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterAi : MonoBehaviour
{
    private Monster monster;
    private MonsterAttack monsterAttack;
    private MonsterAnimator monsterAnimator;

    private Rigidbody2D rb;

    public MonsterState currentState = MonsterState.Idle;

    private void Start()
    {
        monster = GetComponent<Monster>();
        monsterAttack = GetComponent<MonsterAttack>();
        monsterAnimator = GetComponent<MonsterAnimator>();
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
        Transform target = monster.GetTarget();

        if (target == null)
        {
            ChangeState(MonsterState.Idle);
            return;
        }

        // 플레이어 사망 시 추적x
        Player player = target.GetComponent<Player>();
        if (player != null && player.isDead)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            monster.SetTarget(null);
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
                if (distance > data.attackRange && !player.isDead) 
                    ChangeState(MonsterState.Move);
                break;

            case MonsterState.Move:
                if (distance <= data.attackRange)
                    ChangeState(MonsterState.Attack);
                break;

            case MonsterState.Attack:
                if (distance > data.attackRange)
                {
                    ChangeState(MonsterState.Move);
                }
                else
                {
                    if (monsterAttack != null)
                        monsterAttack.TryAttack();
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
                if (monsterAttack != null)
                    monsterAttack.TryAttack();
                break;

            case MonsterState.Dead:
                StopMove();
                if (monsterAttack != null)
                    monsterAttack.StopAttack();
                if (monsterAnimator != null)
                    monsterAnimator.Dead();
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

        if (monsterAnimator != null)
        {
            monsterAnimator.SetMove(rb.velocity.sqrMagnitude > 0.01f);
            monsterAnimator.SetFlip(dir.x);
        }
    }

    private void StopMove()
    {
        if (rb != null)
            rb.velocity = Vector2.zero;

        if (monsterAnimator != null)
            monsterAnimator.SetMove(false);
    }
    
}
