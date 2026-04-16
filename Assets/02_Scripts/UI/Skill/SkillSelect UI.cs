using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SkillSlotUI[] slots;
    [SerializeField] private List<SkillData> skills;
    [SerializeField] private Transform skillParent;
    [SerializeField] private Transform player;

    private List<SkillData> randomSkills = new List<SkillData>();


    private void Awake()
    {
        if (Shared.skillSelectUI == null)
        {
            Shared.skillSelectUI = this;
        }
    }
    public void Open()
    {
        Debug.Log("open");

        panel.SetActive(true);
        Time.timeScale = 0f;

        randomSkills = GetRandomSkills(3);

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlot(randomSkills[i], this);
        }
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SelectSkill(SkillData skill)
    {
        Skill_Manager[] managers = skillParent.GetComponentsInChildren<Skill_Manager>();

        foreach (var  manager in managers)
        {
            if (manager.name == skill.skillName)
            {
                manager.LevelUp();
                Close();
                return;
            }
        }

        GameObject obj = new GameObject(skill.skillName);
        obj .transform.parent = skillParent;

        Skill_Manager newSkill = obj.AddComponent<Skill_Manager>();
        newSkill.Init(skill, player);

        Close();
    }

    private List<SkillData> GetRandomSkills(int count)
    {
        List<SkillData> skillList = new List<SkillData>(skills);
        List<SkillData> randomList = new List<SkillData>();

        while (randomList.Count < count && skillList.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, skillList.Count);
            randomList.Add(skillList[rand]);
            skillList.RemoveAt(rand);
        }
        return randomList;
    }
}
