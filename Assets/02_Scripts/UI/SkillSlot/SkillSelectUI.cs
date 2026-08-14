using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor;

public class SkillSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SkillSlotUI[] slots;
    [SerializeField] private List<ActiveSkillData> activeSkills;
    [SerializeField] private List<PassiveSkillData> passiveSkills;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform skillRoot;
    [SerializeField] private PassiveSkillManager passiveSkillManager;

    private List<SkillSelect> randomSkills = new List<SkillSelect>();

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

    private class SkillSelect
    {
        public ActiveSkillData activeSkill;
        public PassiveSkillData passiveSkill;
        public bool IsPassive => passiveSkill != null;
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
            if (i >= randomSkills.Count)
            {
                slots[i].gameObject.SetActive(false);
                continue;
            }

            slots[i].gameObject.SetActive(true);

            SkillSelect select = randomSkills[i];

            if (select.IsPassive)
            {
                
            }
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
        newSkill.Init(skill, playerTransform);

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
        //ActiveSkillData randomSkill = randomSkills[rand];

        //SelectSkill(randomSkill);
    }

    private List<SkillSelect> GetRandomSkills(int count)
    {
        List<SkillSelect> selectSkill = new List<SkillSelect>();

        List<ActiveSkillData> activeSkillList = new List<ActiveSkillData>();

        for (int i = 0; i < activeSkills.Count; i++)
        {
            ActiveSkillData skill = activeSkills[i];

            if (skill == null) 
                continue;

            int currentLevel = GetSkillLevel(skill);

            // 스킬 최대 레벨 시 제외
            if (currentLevel < skill.maxLevel)
                activeSkillList.Add(skill);
        }

        Player player = playerTransform.GetComponent<Player>();

        int playerLevel = (int)player.stats[StatType.Level];

        if (playerLevel > 5)
        {
            PassiveSkillData passive = GetRandomPassiveSkill();

            if (passive != null)
            {
                SkillSelect select = new SkillSelect();
                select.passiveSkill = passive;
                selectSkill.Add(select);
            }
        }

        while (selectSkill.Count < count && activeSkillList.Count > 0)
        {
            int rand = Random.Range(0, activeSkillList.Count);
            ActiveSkillData active = activeSkillList[rand];

            SkillSelect select = new SkillSelect();
            select.activeSkill = active;
            selectSkill.Add(select);
            activeSkillList.RemoveAt(rand);
        }

        return selectSkill;
    }

    private PassiveSkillData GetRandomPassiveSkill()
    {
        List<PassiveSkillData> passiveSkillList = new List<PassiveSkillData>();

        for (int i = 0; i < passiveSkillList.Count; i++)
        {
            PassiveSkillData skill = passiveSkillList[i];

            if (skill == null)
                continue;

            int currentLevel = passiveSkillManager.GetLevel(skill.passiveType);

            // 스킬 최대 레벨 시 제외
            if (currentLevel < skill.maxLevel)
                passiveSkillList.Add(skill);
        }

        if (passiveSkillList.Count == 0)
            return null;

        int rand = Random.Range(0, passiveSkillList.Count);

        return passiveSkillList[rand];
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
