using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject giveUpConfirmUI;

    [Header("Image Sprites")]
    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite soundOn; 
    [SerializeField] private Sprite soundOff;

    [Header("Skill List UI")]
    [SerializeField] private SkillSlotUI[] activeSlots;
    [SerializeField] private SkillSlotUI[] passiveSlots;
    [SerializeField] private Transform skillRoot;
    [SerializeField] private PassiveSkillManager passiveSkillManager;

    private bool activeSound;
    private Coroutine skillListUICoroutine;


    private void Start()
    {
        activeSound = true;

        if (soundIcon.sprite == null)
            soundIcon.sprite = soundOn;

        if (optionUI != null)
            optionUI.SetActive(false);

        if (giveUpConfirmUI != null)
            giveUpConfirmUI.SetActive(false);

        ClearAllSlots();
    }

    public void Open()
    {
        if (optionUI != null)
            optionUI.SetActive(true);

        if (giveUpConfirmUI != null)
            giveUpConfirmUI.SetActive(false);

        Time.timeScale = 0f;

        if (skillListUICoroutine != null)
            StopCoroutine(skillListUICoroutine);

        skillListUICoroutine = StartCoroutine(SkillListUIUpdate());
    }

    private IEnumerator SkillListUIUpdate()
    {
        if (skillRoot == null)
        {
            ClearAllSlots();
            skillListUICoroutine = null;
            yield break;
        }
        PlayerSkillManager[] managers = skillRoot.GetComponentsInChildren<PlayerSkillManager>();

        for (int i = 0; i < activeSlots.Length; i++)
        {
            if (activeSlots[i] == null)
                continue;

            if (i < managers.Length)
            {
                PlayerSkillManager manager = managers[i];

                if (manager != null && manager.Data != null)
                {
                    activeSlots[i].SetSlot(manager.Data, manager.CurrentLevel);
                }
                else
                    activeSlots[i].ClearSlot();
            }
            else
                activeSlots[i].ClearSlot();

            yield return null;
        }

        Dictionary<PassiveSkillData, int> passiveSkills = passiveSkillManager.GetPassiveSkills();

        int passiveIndex = 0;

        foreach (var skill in passiveSkills)
        {
            if (passiveIndex >= passiveSlots.Length)
                break;

            passiveSlots[passiveIndex].SetSlot(skill.Key, skill.Value);
            passiveIndex++;
        }

        for (int i = passiveIndex; i < passiveSlots.Length; i++)
        {
            passiveSlots[i].ClearSlot();
        }

        skillListUICoroutine = null;
    }

    private void ClearAllSlots()
    {
        if (activeSlots != null)
        {
            for (int i = 0; i < activeSlots.Length; i++)
            {
                if (activeSlots[i] != null)
                    activeSlots[i].ClearSlot();
            }
        }

        if (passiveSlots != null)
        {
            for (int i = 0; i < passiveSlots.Length; i++)
            {
                if (passiveSlots[i] != null)
                    passiveSlots[i].ClearSlot();
            }
        }
    }
    public void OnClickPlayButton()
    {
        if (skillListUICoroutine != null)
        {
            StopCoroutine(skillListUICoroutine);
            skillListUICoroutine = null;
        }

        optionUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OnClickLobbyButton()
    {
        if (giveUpConfirmUI != null)
            giveUpConfirmUI.SetActive(true);
    }

    public void OnClickGiveUpButton()
    {
        Time.timeScale = 1f;

        if (SceneLoadManager.Instance != null)
            SceneLoadManager.Instance.ChangeScene(SceneType.LOBBY, false);
    }

    public void OnClickSoundButton()
    {
        activeSound = !activeSound;

        if (activeSound)
            soundIcon.sprite = soundOn;
        else 
            soundIcon.sprite = soundOff;
    }
}
