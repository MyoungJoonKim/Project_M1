using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSkillManager : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform bossMonster;
    [SerializeField] private Transform targetPlayer;

    [SerializeField] private SkillData skillData;

    public SkillData Data => skillData;

    private readonly List<BossSkillObject> skillObjects = new List<BossSkillObject> ();
    private int currentLevel = 0;
    private bool isSkillLoop = true;

    private Coroutine skillLoop;

    private void Awake()
    {
        if (Shared.bossSkillManager == null) 
            Shared.bossSkillManager = this;
    }

    public void Init(MonsterData monsterData, SkillData _skillData, Transform bossTransform, Transform playerTransform)
    {
        if (monsterData == null) 
            return;

        if (monsterData.monsterType != MonsterType.Boss)
            return;

        if (_skillData == null)
            return;
        
        if (skillLoop != null)
        {
            StopCoroutine(skillLoop);
            skillLoop = null;
        }

        ClearSkillObjects();

        skillData = _skillData;
        bossMonster = bossTransform;
        targetPlayer = playerTransform;

        if (bossMonster == null || targetPlayer == null)
            return;

        isSkillLoop = true;
        skillLoop = StartCoroutine(SkillLoop());
    }

    private void CreateSkill(int summonSkillStage = 0)
    {
        int count = skillData.count[currentLevel];

        while (skillObjects.Count < count)
        {
            GameObject obj = Instantiate(skillData.skillPrefab, bossMonster.position, Quaternion.identity);
            BossSkillObject skill = obj.GetComponent<BossSkillObject>();

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

            float curruntRange = skillData.range[currentLevel];

            if (skillData.skillType == SkillType.Summon)
            {
                curruntRange *= summonSkillStage + 1;
                skillObjects[i].transform.position = bossMonster.position;
            }
            else if (skillData.skillType == SkillType.TargetExplosion)
            {
                if (targetPlayer == null)
                {
                    skillObjects[i].gameObject.SetActive(false);
                    continue;
                }
                skillObjects[i].transform.position = targetPlayer.transform.position;
            }
            else
            {
                skillObjects[i].transform.position = bossMonster.position;
            }

            skillObjects[i].gameObject.SetActive(true);

            skillObjects[i].SetUp(
                bossMonster,
                targetPlayer,
                i,
                count,
                skillData.damage[currentLevel],
                skillData.range[currentLevel],
                skillData.speed[currentLevel],
                skillData.hitInterval[currentLevel],
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

    private IEnumerator SkillLoop()
    {
        yield return new WaitForSeconds(2f);

        while (isSkillLoop)
        {
            if (skillData.skillType == SkillType.Summon)
            {
                yield return StartCoroutine(SummonSkillSequence());
            }
            else
            {
                CreateSkill();
                yield return new WaitForSeconds(skillData.duration);
                ClearSkillObjects();
            } 
            yield return new WaitForSeconds(skillData.cooldown);
        }
    }

    private IEnumerator SummonSkillSequence()
    {
        int stageCount = 3;

        float activeTime = skillData.duration / stageCount;

        for (int stage = 0; stage < stageCount; stage++)
        {
            CreateSkill(stage);
            yield return new WaitForSeconds(activeTime);
            ClearSkillObjects() ;

            if (stage < stageCount - 1)
                yield return new WaitForSeconds(0.25f);
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
}
