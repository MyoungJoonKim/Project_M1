using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EventMonsterAttack : MonoBehaviour
{
    [SerializeField] private Monster monster;

    private float lastAttackTime = -999f;

    private void Awake()
    {
        if (monster == null)
            monster = GetComponent<Monster>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryAttack(collision);
    }


    public void TryAttack(Collider2D collision)
    {
        if (monster == null || monster.isDead)
            return;

        Player target = Shared.battleManager != null ? Shared.battleManager.player : null;
        if (target == null || target.isDead)
            return;

        MonsterData data = monster.GetMonsterData();
        if (data == null) 
            return;

        float cooldown = monster.GetStat(StatType.AttackCooldown);

        if (Time.time < lastAttackTime + cooldown)
            return;

        lastAttackTime = Time.time;

        target.TakeDamage(monster.stats[StatType.Atk], false);
        target.OnHit();
    }
}
