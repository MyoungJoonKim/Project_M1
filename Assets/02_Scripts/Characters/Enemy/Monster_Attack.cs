using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster_Attack : MonoBehaviour
{
    private Monster monster;
    private Monster_Animator monster_Animator;

    private bool isAttacking;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        monster_Animator = GetComponent<Monster_Animator>();
    }

    
    public void TryAttack()
    {
        if (monster == null || monster.isDead)
            return;

        MonsterData data = monster.GetMonsterData();
        if (data == null) 
            return;

        float cooldown = monster.GetStat(StatType.AttackCooldown);

        if (Time.time < lastAttackTime + cooldown)
            return;

        EndAttack();
    }

    public void EndAttack()
    {
        MonsterData data = monster.GetMonsterData();
        if (data == null) 
            return;

        Player target = Shared.battle_Manager != null ? Shared.battle_Manager.player : null;
        if(target == null ||  target.isDead)
            return;

        lastAttackTime = Time.time;
        isAttacking = true;

        if (monster_Animator != null)
        {
            monster_Animator.Attack();
            target.TakeDamage(monster.stats[StatType.Atk]);
        }
        isAttacking = false;
    }
    public void StopAttack()
    {
        isAttacking = false;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
