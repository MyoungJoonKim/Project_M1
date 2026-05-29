using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillData skillData;
    [SerializeField] private Transform player;

    private readonly List<SkillObject> skillObjects = new List<SkillObject> ();
    private int currentLevel = 1;
    private Coroutine skillLoop;

    public int CurrentLevel => currentLevel;
    public SkillData Data => skillData;


    private void Awake()
    {
        Shared.skillManager = this;
    }

    private void OnDestroy()
    {
        if (Shared.skillManager == this)
            Shared.skillManager = null;
    }

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
        else if (skillData.skillType == SkillType.Summon)
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

        if (skillData.skillType == SkillType.TargetExplosion)
        {
            for (int i = 0; i < count; i++)
            {
                Monster target = GetRandomMonster();

                if (target == null)
                    return;

                Vector3 spawnPos = target.transform.position;

                GameObject obj = Instantiate(skillData.skillPrefab, spawnPos, Quaternion.identity);
                SkillObject skill = obj.GetComponent<SkillObject>();

                skillObjects.Add(skill);

                skill.SetUp(
                    player,
                    i,
                    count,
                    skillData.damage[currentLevel - 1],
                    skillData.range[currentLevel - 1],
                    skillData.radius[currentLevel - 1],
                    skillData.speed[currentLevel - 1],
                    skillData.hitInterval[currentLevel - 1],
                    skillData.skillType
                );
            }

            return;
        }

        if (skillData.skillType == SkillType.Summon)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPos = GetRandomPosition();

                GameObject obj = Instantiate(skillData.skillPrefab, spawnPos, Quaternion.identity);
                SkillObject skill = obj.GetComponent<SkillObject>();
                skillObjects.Add(skill);

                skill.SetUp(
                    player,
                    i,
                    count,
                    skillData.damage[currentLevel - 1],
                    skillData.range[currentLevel - 1],
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
                skillData.range[currentLevel - 1],
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
            else if(skillData.skillType == SkillType.Summon)
            {
                CreateSkill();
                yield return new WaitForSeconds(skillData.duration);
                ClearSkillObjects();
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

    private void ClearSkillObjects()
    {
        for (int i = 0; i < skillObjects.Count; i++)
        {
            if (skillObjects[i] != null)
                Destroy(skillObjects[i].gameObject);
        }

        skillObjects.Clear();
    }

    
    public void StopAllSkills()
    {
        StopAllCoroutines();
        skillLoop = null;

        for (int i = skillObjects.Count - 1; i >= 0; i--)
        {
            if (skillObjects[i] == null)
                continue;

            skillObjects[i].StopSkill();
        }

        skillObjects.Clear();
    }

    public Monster GetRandomMonster()
    {
        List<Monster> list = new List<Monster>();

        foreach (Monster monster in Shared.spawnManager.GetActiveMonsters())
        {
            if (monster == null || monster.isDead)
                continue;

            float distance = Vector2.Distance(player.position, monster.transform.position);

            if (distance < skillData.range[currentLevel - 1])
                list.Add(monster);
        }

        if (list.Count == 0)
            return null;

        return list[Random.Range(0, list.Count)];
    }

    private Vector3 GetRandomPosition()
    {
        Vector2 offset = Random.insideUnitCircle * skillData.range[currentLevel - 1];
        return player.position + new Vector3(offset.x, offset.y, 0f);
    }
}
