using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptionUI : MonoBehaviour
{
    [Header("Image Sprites")]
    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;


    private bool activeSound;

    private void Start()
    {
        activeSound = true;

        if (soundIcon.sprite == null)
            soundIcon.sprite = soundOn;
    }

    public void OnClickMasterMuteButton()
    {
        activeSound = !activeSound;

        if (activeSound)
            soundIcon.sprite = soundOn;
        else
            soundIcon.sprite = soundOff;

        SoundManager.Instance.MuteMaster(!activeSound);
        SoundManager.Instance.PlayButtonClick();
    }

    public void OnClickBGMMuteButton()
    {
        activeSound = !activeSound;

        if (activeSound)
            soundIcon.sprite = soundOn;
        else
            soundIcon.sprite = soundOff;

        SoundManager.Instance.MuteBGM(!activeSound);
        SoundManager.Instance.PlayButtonClick();
    }

    public void OnClickSFXMuteButton()
    {
        activeSound = !activeSound;

        if (activeSound)
            soundIcon.sprite = soundOn;
        else
            soundIcon.sprite = soundOff;

        SoundManager.Instance.MuteSFX(!activeSound);
        SoundManager.Instance.PlayButtonClick();
    }

}
