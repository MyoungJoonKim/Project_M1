using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster_Attack : MonoBehaviour
{
    private Monster_Movement monster_Movement;
    private Monster_Animator monster_Animator;

    private Transform target;
    private Coroutine attackRoutine;
    private float attackRange = 5f;
    private float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        monster_Movement = GetComponent<Monster_Movement>();
        monster_Animator = GetComponent<Monster_Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (monster_Movement == null)
            Debug.Log("movement null");
        if (monster_Animator == null)
            Debug.Log("animator null");
        if (target == null)
            Debug.Log("target null");
    }

    private void Start()
    {
        StartCoroutine(AttackRange());
    }

    // 공격범위 검사 코루틴
    IEnumerator AttackRange()
    {
        while (true)
        {
            float distance = Vector2.Distance(target.position, this.transform.position);

            if (distance <= attackRange)
            {
                if (attackRoutine == null)
                    attackRoutine = StartCoroutine(TryAttack());
            }
            else if (distance > attackRange)
            {
                if (attackRoutine != null)
                {
                    StopCoroutine(attackRoutine);
                    attackRoutine = null;
                }
            }
            yield return null;
        }
    }

    // 공격 루프 코루틴
    IEnumerator TryAttack()
    {
        while (true)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
            yield return null;
        }
    }

    // 공격 애니메이션 동작
    public void Attack()
    {
        monster_Animator.Attack();
    }
}
