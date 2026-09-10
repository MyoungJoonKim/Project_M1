using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptionUI : MonoBehaviour
{
    [Header("Image Sprites")]
    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;

    [Header("Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;


    private bool activeSound;

    private void Start()
    {
        activeSound = true;

        if (soundIcon.sprite == null)
            soundIcon.sprite = soundOn;

        SoundSetting();

        if (masterSlider != null)
        {
            masterSlider.onValueChanged.RemoveAllListeners();
            masterSlider.onValueChanged.AddListener(OnChangedMasterVolume);
        }

        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveAllListeners();
            bgmSlider.onValueChanged.AddListener(OnChangedBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(OnChangedSFXVolume);
        }
    }

    private void SoundSetting()
    {
        if (UserManager.Instance == null)
            return;

        if (masterSlider != null)
            masterSlider.SetValueWithoutNotify(UserManager.Instance.masterVolume);

        if (bgmSlider != null)
            bgmSlider.SetValueWithoutNotify(UserManager.Instance.bgmVolume);

        if (sfxSlider != null)
            sfxSlider.SetValueWithoutNotify(UserManager.Instance.sfxVolume);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetMasterVolume(UserManager.Instance.masterVolume);
            SoundManager.Instance.SetBGMVolume(UserManager.Instance.bgmVolume);
            SoundManager.Instance.SetSFXVolume(UserManager.Instance.sfxVolume);
        }
    }
    public void OnChangedMasterVolume(float value)
    {
        if (value <= 0.0001f)
            value = 0.0001f;

        UserManager.Instance.SetMasterVolume(value);
        SoundManager.Instance.SetMasterVolume(value);
    }

    public void OnChangedBGMVolume(float value)
    {
        if (value <= 0.0001f)
            value = 0.0001f;

        UserManager.Instance.SetBGMVolume(value);
        SoundManager.Instance.SetBGMVolume(value);
    }

    public void OnChangedSFXVolume(float value)
    {
        if (value <= 0.0001f)
            value = 0.0001f;

        UserManager.Instance.SetSFXVolume(value);
        SoundManager.Instance.SetSFXVolume(value);
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
