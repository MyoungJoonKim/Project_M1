using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PassiveSkillManager : MonoBehaviour
{
    private Dictionary<PassiveType, int> passiveLevels = new Dictionary<PassiveType, int>();

    public float ExpGainRate { get; private set; } = 1f;
    public float SkillDamageRate { get; private set; } = 1f;
    public float DamageReductionRate { get; private set; } = 1f;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();

    }

    public void LevelUp(PassiveType type)
    {
        int level = GetLevel(type);

        if (level > 5)
            return;

        passiveLevels[type] = level + 1;

        ApplyPassive(type);
    }

    public int GetLevel(PassiveType type)
    {
        if (passiveLevels.ContainsKey(type))
            return passiveLevels[type];

        return 0;
    }

    private void ApplyPassive(PassiveType type)
    {
        int level = passiveLevels[type];

        switch (type)
        {
            case PassiveType.ExpBonus:
                ApplyValue(level);
                break;
            case PassiveType.DamageReduction:
                ApplyValue(level);
                break;
            case PassiveType.DamageBonus:
                ApplyValue(level);
                break;
            case PassiveType.PickupRange:
                ApplyValue(level);
                break;
            case PassiveType.MoveSpeed:
                ApplyValue(level);
                break;
        }
    }

    private void ApplyValue(int level)
    {

    }
}
