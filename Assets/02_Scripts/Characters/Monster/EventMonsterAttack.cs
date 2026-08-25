using UnityEngine;

public class EventMonsterAttack : MonoBehaviour
{
    [Header("Monster")]
    [SerializeField] private Monster monster;

    private BattleManager battleManager;

    private static float lastAttackTime = -999f;

    private void Awake()
    {
        if (monster == null)
            monster = GetComponent<Monster>();

        battleManager = GetComponent<BattleManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryAttack(collision);
    }


    public void TryAttack(Collider2D collision)
    {
        if (battleManager == null || !battleManager.isBattlePlaying)
            return;

        if (monster == null || monster.isDead)
            return;

        Player target = collision.GetComponentInParent<Player>();

        if (target == null || target.isDead)
            return;

        MonsterData data = monster.GetMonsterData();

        if (data == null) 
            return;

        float cooldown = monster.GetStat(StatType.AttackCooldown);

        if (Time.time < lastAttackTime + cooldown)
            return;

        lastAttackTime = Time.time;

        float damage = monster.stats[StatType.Atk];

        PassiveSkillManager passiveSkillManager = target.GetComponent<PassiveSkillManager>();

        if (passiveSkillManager != null)
        {
            damage *= 1f - passiveSkillManager.DamageReductionRate;
        }

        target.TakeDamage(damage, false);
        target.OnHit();
    }

    public static void ResetAttackTime()
    {
        lastAttackTime = -999f;
    }
}
