using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Manager : MonoBehaviour
{
    [SerializeField] private SkillData skillData;
    [SerializeField] private Transform player;

    private readonly List<SkillObject> skillObjects = new List<SkillObject> ();
    private int currentLevel = 1;
    private Coroutine skillLoop;

    public int CurrentLevel => currentLevel;
    public SkillData Data => skillData;

    public void Init(SkillData data, Transform _player)
    {
        skillData = data;
        player = _player;
        currentLevel = 1;

        CreateSkill();

        if (skillData.skillType == SkillType.TargetExplosion)
        {
            if (skillLoop != null) 
                StopCoroutine(skillLoop);

            skillLoop = StartCoroutine(SkillLoop());
        }
        else
            CreateSkill();
    }

    public void LevelUp()
    {
        currentLevel++;

        if (currentLevel > skillData.maxLevel)
            currentLevel = skillData.maxLevel;

        CreateSkill();
    }

    private void CreateSkill()
    {
        for (int i = 0; i < skillObjects.Count; i++)
        {
            Destroy(skillObjects[i].gameObject);
        }
        skillObjects.Clear();

        int count = skillData.count[currentLevel - 1];

        if (skillData.skillType == SkillType.TargetExplosion) // iceExplosion 스킬만 따로 처리
        {
            for (int i = 0; i < count; i++)
            {
                Monster target = GetRandomMonster();

                if (target == null)
                {
                    Debug.Log("null target");
                    return;
                }
                Debug.Log("target");

                Vector3 spawnPos = target.transform.position;

                GameObject obj = Instantiate(skillData.skillPrefab, spawnPos, Quaternion.identity);
                SkillObject skill = obj.GetComponent<SkillObject>();

                skill.SetUp(
                    player,
                    i,
                    count,
                    skillData.damage[currentLevel - 1],
                    skillData.radius[currentLevel - 1],
                    skillData.speed[currentLevel - 1],
                    skillData.hitInterval[currentLevel - 1],
                    skillData.skillType
                );
            }
            return;
        }

        while (skillObjects.Count < count)
        {
            GameObject obj = Instantiate(skillData.skillPrefab, player.position, Quaternion.identity);
            SkillObject skill = obj.GetComponent<SkillObject>();
            skillObjects.Add(skill);
        }

        for (int i = 0; i < skillObjects.Count; i++)
        {
            bool active = i < count;
            skillObjects[i].gameObject.SetActive(active);

            if (!active) continue;

            skillObjects[i].SetUp(
                player,
                i,
                count,
                skillData.damage[currentLevel - 1],
                skillData.radius[currentLevel - 1],
                skillData.speed[currentLevel - 1],
                skillData.hitInterval[currentLevel - 1],
                skillData.skillType
                );
        }
    }

    private IEnumerator SkillLoop()
    {
        while (true)
        {
            if (skillData.skillType == SkillType.TargetExplosion)
            {
                CreateSkill();
                yield return new WaitForSeconds(skillData.cooldown);
            }
            else
            {
                SetSkillAttack(true);
                yield return new WaitForSeconds(skillData.duration);
                SetSkillAttack(false);
                yield return new WaitForSeconds(skillData.cooldown);
            }
        }
    }

    private void SetSkillAttack(bool value)
    {
        for (int i = 0;i < skillObjects.Count;i++)
        {
            if (skillObjects[i].gameObject.activeSelf)
                skillObjects[i].SetAttack(value);
        }
    }

    private Monster GetRandomMonster()
    {
        List<Monster> list = new List<Monster>();

        foreach (Monster monster in Shared.battle_Manager.monsters)
        {
            if (monster == null || monster.isDead)
                continue;

            float distance = Vector2.Distance(player.position, monster.transform.position);

            if (distance < skillData.radius[currentLevel - 1])
                list.Add(monster);
        }

        if (list.Count == 0)
            return null;

        return list[Random.Range(0, list.Count)];
    }


}
