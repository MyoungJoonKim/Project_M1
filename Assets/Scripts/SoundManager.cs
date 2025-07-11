using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider[] audioSliders;
    public Toggle[] muteToggles;
    public Text[] volumeTexts;

    public string[] mixerParams;
    public float[] saveVolumes;

    void Start()
    {
        int count = audioSliders.Length;
        saveVolumes = new float[count];

        for (int i = 0; i < count; i++)
        {
            int index = i;
            saveVolumes[index] = audioSliders[index].value;

            // 초기 볼륨 텍스트
            float percent = Mathf.Clamp01((saveVolumes[index] + 40f) / 40f) * 100f;
            volumeTexts[index].text = $"{MathF.Round(percent)}%";

            // 슬라이더 볼륨 변경 시
            audioSliders[index].onValueChanged.AddListener((volume) =>
                {
                if (!muteToggles[index].isOn)
                {
                    saveVolumes[index] = volume;
                        audioMixer.SetFloat(mixerParams[index], volume);

                        float percent = Mathf.Clamp01((audioSliders[index].value + 40f) / 40f) * 100f;
                            volumeTexts[index].text = $"{MathF.Round(percent)}%";
                }
            });
            // 토글 음소거 적용 or 해제 시 복원
            muteToggles[index].onValueChanged.AddListener((bool isOn) =>
            {
                MuteToggle(index, isOn);
            });

            audioMixer.SetFloat(mixerParams[index], muteToggles[index].isOn ? -80f : audioSliders[index].value);
        }
    }

    public void MuteToggle(int index, bool mute)
    {
        if (mute)
        {
            audioMixer.SetFloat(mixerParams[index], -80f);
        }
        else
        {
            audioMixer.SetFloat(mixerParams[index], saveVolumes[index]);
        }
    }

}
