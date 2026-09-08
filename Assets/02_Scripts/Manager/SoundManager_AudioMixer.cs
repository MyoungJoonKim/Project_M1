using UnityEngine;
using UnityEngine.Audio;

public partial class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;


    public void MuteMaster(bool mute)
    {
        audioMixer.SetFloat("MasterVolume", mute ? -80f : 0f);
    }
    public void MuteBGM(bool mute)
    {
        audioMixer.SetFloat("BGMVolume", mute ? -80f : 0f);
    }
    public void MuteSFX(bool mute)
    {
        audioMixer.SetFloat("SFXVolume", mute ? -80f : 0f);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
    }
    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20f);
    }
    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
    }
}
