using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SkillSlotUI[] slots;
    [SerializeField] private List<SkillData> skills;
    [SerializeField] private Transform player;
    [SerializeField] private Transform skillRoot;

    private List<SkillData> randomSkills = new List<SkillData>();

    [Header("Skill Select Timer")]
    [SerializeField] private float SelectTime = 30f;
    [SerializeField] private TMP_Text selectTimeText;

    private float currentSelectTime;


    private Coroutine selectTimeCoroutine;
    public bool isSelectUI;

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
        isSelectUI = true;
        currentSelectTime = SelectTime;

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

        if (selectTimeCoroutine != null)
            StopCoroutine(selectTimeCoroutine);

        selectTimeCoroutine = StartCoroutine(SelectTimeUpdate());
    }

    public void Close()
    {
        isSelectUI = false;

        if (selectTimeCoroutine != null)
        {
            StopCoroutine(selectTimeCoroutine);
            selectTimeCoroutine = null;
        }

        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SelectSkill(SkillData skill)
    {
        if (!isSelectUI)
            return;

        SkillManager[] managers = skillRoot.GetComponentsInChildren<SkillManager>();

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

        SkillManager newSkill = obj.AddComponent<SkillManager>();
        newSkill.Init(skill, player);

        Close();
    }
    private void AutoSelectRandomSkill()
    {
        if (!isSelectUI)
            return;

        if (randomSkills == null || randomSkills.Count == 0)
        {
            Close();
            return;
        }

        int rand = Random.Range(0, randomSkills.Count);
        SkillData randomSkill = randomSkills[rand];

        SelectSkill(randomSkill);
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
        SkillManager[] managers = skillRoot.GetComponentsInChildren<SkillManager>();

        foreach (var manager in managers)
        {
            if (manager.Data == skill)
                return manager.CurrentLevel;
        }
        return 0;
    }
    private IEnumerator SelectTimeUpdate()
    {
        while (isSelectUI)
        {
            currentSelectTime -= Time.unscaledDeltaTime;

            if (selectTimeText != null)
            {
                int second = Mathf.CeilToInt(currentSelectTime);
                selectTimeText.text = $"Time {second:00}";
            }

            if (currentSelectTime <= 0f)
            {
                AutoSelectRandomSkill();
                yield break;
            }
            yield return null;
        }

        selectTimeCoroutine = null;
    }
    
}
