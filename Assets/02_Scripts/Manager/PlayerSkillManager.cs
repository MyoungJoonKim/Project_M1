using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("Player Transform")]
    [SerializeField] private Transform player;

    [Header("Active Skill Data")]
    [SerializeField] private ActiveSkillData skillData;
    
    
    private readonly List<PlayerSkillObject> skillObjects = new List<PlayerSkillObject> ();

    private int currentLevel = 1;

    private Coroutine skillLoop;

    public int CurrentLevel => currentLevel;
    public ActiveSkillData Data => skillData;


    private void OnDestroy()
    {
        if (Shared.playerSkillManager == this)
            Shared.playerSkillManager = null;
    }

    public void Init(ActiveSkillData data, Transform _player)
    {
        skillData = data;
        player = _player;
        currentLevel = 1;
        
        if (skillData.skillType == SkillType.EventSummon)
            return;

        CreateSkill();

        if (skillData.skillType == SkillType.TargetExplosion || 
            skillData.skillType == SkillType.Summon || 
            skillData.skillType == SkillType.Direction)
        {
            if (skillLoop != null) 
                StopCoroutine(skillLoop);

            skillLoop = StartCoroutine(SkillLoop());
        }
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
        int count = skillData.count[currentLevel - 1];

        while (skillObjects.Count < count)
        {
            GameObject obj = Instantiate(skillData.skillPrefab, player.position, Quaternion.identity);
            PlayerSkillObject skill = obj.GetComponent<PlayerSkillObject>();

            if (skill == null)
            {
                Destroy(obj);
                continue;
            }
            skillObjects.Add(skill);
        }

        for (int i = 0; i < skillObjects.Count; i++)
        {
            bool active = i < count;

            if (!active)
            {
                skillObjects[i].gameObject.SetActive(false);
                continue;
            }

            if (skillData.skillType == SkillType.TargetExplosion)
            {
                Monster target = GetRandomMonster();

                if (target == null)
                {
                    skillObjects[i].gameObject.SetActive(false);
                    continue;
                }

                skillObjects[i].transform.position = target.transform.position;
            }
            else if (skillData.skillType == SkillType.Summon)
            {
                skillObjects[i].transform.position = GetRandomPosition();
            }
            else if (skillData.skillType == SkillType.EventSummon)
            {
                skillObjects[i].transform.position = Shared.spawnManager.GetRandomPosition();
            }
            else
            {
                skillObjects[i].transform.position = player.position;
            }

            skillObjects[i].gameObject.SetActive(true);

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

            skillObjects[i].SetAttack(true);
        } 
    }
    private void ClearSkillObjects()
    {
        for (int i = 0; i < skillObjects.Count; i++)
        {
            if (skillObjects[i] == null)
                continue;

            skillObjects[i].StopSkill();
            skillObjects[i].gameObject.SetActive(false);
        }
    }

    public void CreateEventSkill()
    {
        if (skillData == null)
            return;

        if (skillData.skillType != SkillType.EventSummon)
            return;

        if (skillLoop != null)
        {
            StopCoroutine(skillLoop);
            skillLoop = null;
        }

        //CreateSkill();
        skillLoop = StartCoroutine(EventSkillLoop());
    }

    private IEnumerator SkillLoop()
    {
        while (true)
        {
            CreateSkill();
            yield return new WaitForSeconds(skillData.duration);
            ClearSkillObjects();
            yield return new WaitForSeconds(skillData.cooldown);
        }
    }

    private IEnumerator EventSkillLoop()
    {
        float timer = 0;

        while (timer < skillData.duration)
        {
            CreateSkill();
            yield return new WaitForSeconds(skillData.cooldown);
            timer += skillData.cooldown;
        }

        ClearSkillObjects();
        skillLoop = null;
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
