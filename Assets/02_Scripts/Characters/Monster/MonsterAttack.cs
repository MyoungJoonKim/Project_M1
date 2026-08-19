using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private Monster monster;
    private MonsterAnimator monsterAnimator;

    private bool isAttacking;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        monsterAnimator = GetComponent<MonsterAnimator>();
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

        Player target = Shared.battleManager != null ? Shared.battleManager.player : null;

        if (target == null ||  target.isDead)
            return;

        float damage = monster.stats[StatType.Atk];

        PassiveSkillManager passiveSkillManager = target.GetComponent<PassiveSkillManager>();

        if (passiveSkillManager != null)
        {
            damage *= 1f - passiveSkillManager.DamageReductionRate;
        }
        
        lastAttackTime = Time.time;
        isAttacking = true;

        if (monsterAnimator != null)
        {
            monsterAnimator.Attack();
            target.TakeDamage(damage, false);
            target.OnHit();
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
