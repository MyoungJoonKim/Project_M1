using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] private SkillSlotUI[] slots;
    [SerializeField] private Transform skillRoot;

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

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            if (i < managers.Length)
            {
                PlayerSkillManager manager = managers[i];

                if (manager != null && manager.Data != null)
                {
                    slots[i].SetSlot(manager.Data, manager.CurrentLevel);
                }
                else
                    slots[i].ClearSlot();
            }
            else
                slots[i].ClearSlot();

            yield return null;
        }
        skillListUICoroutine = null;
    }

    private void ClearAllSlots()
    {
        if (slots == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
                slots[i].ClearSlot();
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

        if (Shared.sceneLoadManager != null)
            Shared.sceneLoadManager.ChangeScene(SceneType.LOBBY, false);
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
