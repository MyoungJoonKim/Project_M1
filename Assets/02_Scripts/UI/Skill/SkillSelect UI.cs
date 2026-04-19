using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SkillSlotUI[] slots;
    [SerializeField] private List<SkillData> skills;
    [SerializeField] private Transform skillRoot;
    [SerializeField] private Transform player;

    private List<SkillData> randomSkills = new List<SkillData>();


    private void Awake()
    {
        if (Shared.skillSelectUI == null)
        {
            Shared.skillSelectUI = this;
        }
    }

    private void Start()
    {
        panel.SetActive(false);
    }
    public void Open()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;

        randomSkills = GetRandomSkills(3);

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < randomSkills.Count)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].SetSlot(randomSkills[i], this);
            }
            else
                slots[i].gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SelectSkill(SkillData skill)
    {
        Skill_Manager[] managers = skillRoot.GetComponentsInChildren<Skill_Manager>();

        foreach (var  manager in managers)
        {
            if (manager.Data == skill)
            {
                manager.LevelUp();
                Close();
                return;
            }
        }

        GameObject obj = new GameObject(skill.skillName);
        obj.transform.parent = skillRoot;
        obj.transform.localPosition = Vector3.zero;

        Skill_Manager newSkill = obj.AddComponent<Skill_Manager>();
        newSkill.Init(skill, player);

        Close();
    }

    private List<SkillData> GetRandomSkills(int count)
    {
        List<SkillData> skillList = new List<SkillData>();

        for (int i = 0; i < skills.Count; i++)
        {
            SkillData skill = skills[i];

            if (skill == null) 
                continue;

            int currentLevel = GetSkillLevel(skill);

            if (currentLevel < skill.maxLevel)
                skillList.Add(skill);
        }

        List<SkillData> randomList = new List<SkillData>();

        while (randomList.Count < count && skillList.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, skillList.Count);
            randomList.Add(skillList[rand]);
            skillList.RemoveAt(rand);
        }
        return randomList;
    }

    public int GetSkillLevel(SkillData skill)
    {
        Skill_Manager[] managers = skillRoot.GetComponentsInChildren<Skill_Manager>();

        foreach (var manager in managers)
        {
            if (manager.Data == skill)
                return manager.CurrentLevel;
        }
        return 0;
    }
}
