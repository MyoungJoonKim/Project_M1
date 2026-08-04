using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillManager : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform bossMonster;
    [SerializeField] private Transform targetPlayer;

    private Coroutine skillLoop;

    private SkillData projectionSkill;
    private SkillData targetExplosionSkill;
    private SkillData summonSkill;

    private readonly List<BossSkillObject> bossSkills = new List<BossSkillObject> ();
    private int skillIndex = 0;
    private bool isSkillLoop = true;

    public void Init(MonsterData monsterData, Transform bossTransform, Transform playerTransform)
    {
        if (monsterData == null) 
            return;

        if (monsterData.monsterType != MonsterType.Boss)
            return;

        bossMonster = bossTransform;
        targetPlayer = playerTransform;

        projectionSkill = monsterData.projectionSkill;
        targetExplosionSkill = monsterData.targetExplosionSkill;
        summonSkill = monsterData.summonSkill;

        if (bossMonster == null || targetPlayer == null)
            return;

        isSkillLoop = true;
       // skillLoop = StartCoroutine(SkillLoop());
    }


    //private void CreateSkill()
    //{
    //    int count = skillData.count[currentLevel - 1];

    //    while (skillObjects.Count < count)
    //    {
    //        GameObject obj = Instantiate(skillData.skillPrefab, bossMonster.position, Quaternion.identity);
    //        SkillObject skill = obj.GetComponent<SkillObject>();

    //        if (skill == null)
    //        {
    //            Destroy(obj);
    //            continue;
    //        }
    //        skillObjects.Add(skill);
    //    }

    //    for (int i = 0; i < skillObjects.Count; i++)
    //    {
    //        bool active = i < count;

    //        if (!active)
    //        {
    //            skillObjects[i].gameObject.SetActive(false);
    //            continue;
    //        }

    //        if (skillData.skillType == SkillType.TargetExplosion)
    //        {
    //            if (targetPlayer == null)
    //            {
    //                skillObjects[i].gameObject.SetActive(false);
    //                continue;
    //            }

    //            skillObjects[i].transform.position = targetPlayer.transform.position;
    //        }
    //        else if (skillData.skillType == SkillType.Summon)
    //        {
    //            skillObjects[i].transform.position = GetRandomPosition();
    //        }
    //        else if (skillData.skillType == SkillType.EventSummon)
    //        {
    //            skillObjects[i].transform.position = Shared.spawnManager.GetRandomPosition();
    //        }
    //        else
    //        {
    //            skillObjects[i].transform.position = bossMonster.position;
    //        }

    //        skillObjects[i].gameObject.SetActive(true);

    //        skillObjects[i].SetUp(
    //            bossMonster,
    //            i,
    //            count,
    //            skillData.damage[currentLevel - 1],
    //            skillData.range[currentLevel - 1],
    //            skillData.radius[currentLevel - 1],
    //            skillData.speed[currentLevel - 1],
    //            skillData.hitInterval[currentLevel - 1],
    //            skillData.skillType
    //            );

    //        skillObjects[i].SetAttack(true);
    //    } 
    //}
    //private void ClearSkillObjects()
    //{
    //    for (int i = 0; i < skillObjects.Count; i++)
    //    {
    //        if (skillObjects[i] == null)
    //            continue;

    //        skillObjects[i].StopSkill();
    //        skillObjects[i].gameObject.SetActive(false);
    //    }
    //}

    //private IEnumerator SkillLoop()
    //{
    //    yield return new WaitForSeconds(2f);

    //    while (isSkillLoop)
    //    {
    //        CreateSkill();
    //        yield return new WaitForSeconds(skillData.duration);
    //        ClearSkillObjects();
    //        yield return new WaitForSeconds(skillData.cooldown);
    //    }
    //}

    //public void StopAllSkills()
    //{
    //    StopAllCoroutines();
    //    skillLoop = null;

    //    for (int i = skillObjects.Count - 1; i >= 0; i--)
    //    {
    //        if (skillObjects[i] == null)
    //            continue;

    //        skillObjects[i].StopSkill();
    //    }

    //    skillObjects.Clear();
    //}

    //private Vector3 GetRandomPosition()
    //{
    //    Vector2 offset = Random.insideUnitCircle * bossSkills.range[currentLevel - 1];
    //    return bossMonster.position + new Vector3(offset.x, offset.y, 0f);
    //}
}
