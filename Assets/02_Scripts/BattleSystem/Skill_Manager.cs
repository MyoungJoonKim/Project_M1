using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Manager : MonoBehaviour
{
    [SerializeField] private SkillData skillData;
    [SerializeField] private Transform player;

    private int currentLevel = 1;
    private readonly List<SkillObject> skillObjects = new List<SkillObject> ();

    private void Start()
    {
        CreateSkill();
        StartCoroutine(SkillLoop());
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
            SetSkillAttack(true);
            yield return new WaitForSeconds(skillData.duration);

            SetSkillAttack(false);
            yield return new WaitForSeconds(skillData.cooldown);
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


}
