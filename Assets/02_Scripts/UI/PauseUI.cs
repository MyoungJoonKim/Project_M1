using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject optionUI;

    [Header("Image Sprites")]

    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite soundOn; 
    [SerializeField] private Sprite soundOff;


    private bool activeSound;

    private SkillData skillData;

    public void OnClickPlayButton()
    {
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
