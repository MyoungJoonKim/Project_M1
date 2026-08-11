using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SkillSlotUI[] slots;
    [SerializeField] private List<ActiveSkillData> activeSkills;
    [SerializeField] private List<PassiveSkillData> passiveSkills;
    [SerializeField] private Transform player;
    [SerializeField] private Transform skillRoot;

    private List<ActiveSkillData> randomSkills = new List<ActiveSkillData>();

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
                SetSkillSlot(i, randomSkills[i]);
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


    private void SetSkillSlot(int index, ActiveSkillData data)
    {
        int level = GetSkillLevel(data);

        slots[index].SetSlot(data, level);

        Button button = slots[index].GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => SelectSkill(data));
        }
    }
    private void SelectSkill(ActiveSkillData skill)
    {
        if (!isSelectUI)
            return;

        if (skill == null)
            return;

        PlayerSkillManager[] managers = skillRoot.GetComponentsInChildren<PlayerSkillManager>();

        foreach (var manager in managers)
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

        PlayerSkillManager newSkill = obj.AddComponent<PlayerSkillManager>();
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
        ActiveSkillData randomSkill = randomSkills[rand];

        SelectSkill(randomSkill);
    }

    private List<ActiveSkillData> GetRandomSkills(int count)
    {
        List<ActiveSkillData> skillList = new List<ActiveSkillData>();

        for (int i = 0; i < activeSkills.Count; i++)
        {
            ActiveSkillData skill = activeSkills[i];

            if (skill == null) 
                continue;

            int currentLevel = GetSkillLevel(skill);

            if (currentLevel < skill.maxLevel)
                skillList.Add(skill);
        }

        List<ActiveSkillData> randomList = new List<ActiveSkillData>();

        while (randomList.Count < count && skillList.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, skillList.Count);
            randomList.Add(skillList[rand]);
            skillList.RemoveAt(rand);
        }
        return randomList;
    }

    public int GetSkillLevel(ActiveSkillData skill)
    {
        PlayerSkillManager[] managers = skillRoot.GetComponentsInChildren<PlayerSkillManager>();

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
