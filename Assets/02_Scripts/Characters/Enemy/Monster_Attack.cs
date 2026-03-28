using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster_Attack : MonoBehaviour
{
    private Monster_Movement monster_Movement;
    private Monster_Animator monster_Animator;

    public bool inAttackRange = false;
    private Coroutine attackRoutine;
    private float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        monster_Movement = GetComponent<Monster_Movement>();
        monster_Animator = GetComponent<Monster_Animator>();

        if (monster_Movement == null)
            Debug.Log("movement null");
        if (monster_Animator == null)
            Debug.Log("animator null");
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

    // 충돌 시 공격 루프 동작
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inAttackRange = true;

            if (attackRoutine == null)
                attackRoutine = StartCoroutine(TryAttack());
        }
    }

    // 충돌 중단 시 공격 루프 종료
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inAttackRange = false;

            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }
        }
    }
    
}
