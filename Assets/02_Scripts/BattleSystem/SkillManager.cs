using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
            SkillObject skill = obj.GetComponent<SkillObject>();

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
            else if (skillData.skillType == SkillType.Summon ||
                skillData.skillType == SkillType.EventSummon) // 이벤트 성공 시 스킬가져오기 수정할 것.
            {
                skillObjects[i].transform.position = GetRandomPosition();
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
