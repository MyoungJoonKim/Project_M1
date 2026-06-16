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

    [Header("Image Sprites")]

    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite soundOn; 
    [SerializeField] private Sprite soundOff;


    [SerializeField] private SkillSlotUI[] slots;
    [SerializeField] private Transform skillRoot;

    private bool activeSound;
    private Coroutine skillListUICoroutine;


    private void Start()
    {
        activeSound = true;
        if (soundIcon.sprite == null)
            soundIcon.sprite = soundOn;

        if (skillListUICoroutine != null)
            StopCoroutine(skillListUICoroutine);

        skillListUICoroutine = StartCoroutine(SkillListUIUpdate());

    }
    private IEnumerator SkillListUIUpdate()
    {
        SkillManager[] managers = skillRoot.GetComponentsInChildren<SkillManager>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < managers.Length)
            {
                slots[i].SetSlot(managers[i].Data, null);
            }
            else
                slots[i].gameObject.SetActive(false);
        }
        yield return null;

        skillListUICoroutine = null;
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
