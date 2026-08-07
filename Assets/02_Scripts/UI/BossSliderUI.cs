using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossSliderUI : MonoBehaviour
{
    [Header("Boss Monster")]
    [SerializeField] private Monster monster;

    [Header("Slider Bar")]
    [SerializeField] private Slider bossHpBar;

    [Header("Text")]
    [SerializeField] private TMP_Text bossText;

    private void Start()
    {
        SetActiveBar(false);
    }
    public void Init(Monster bossMonster)
    {
        if (bossMonster == null)
            return;

        MonsterData data = bossMonster.GetMonsterData();

        if (data == null)
            return;

        if (data.monsterType != MonsterType.Boss)
            return;

        // 기존 보스가 연결되어 있으면 해제
        if (monster != null) 
            monster.StatBarChange -= ResetBars;

        monster = bossMonster;
        monster.StatBarChange += ResetBars;

        ResetBars();
        SetActiveBar(true);
    }

    private void OnDestroy()
    {
        if (monster != null)
            monster.StatBarChange -= ResetBars;
    }

    private void ResetBars()
    {
        if (monster == null)
            return;

        bossHpBar.maxValue = monster.maxStats[MaxStatType.MaxHp];
        bossHpBar.value = monster.stats[StatType.Hp];

        if (monster.isDead)
            SetActiveBar(false);
    }

    public void SetActiveBar(bool value)
    {
        if (bossHpBar != null)
            bossHpBar.gameObject.SetActive(value);

        if (bossText != null)
            bossText.gameObject.SetActive(value);
    }


}
