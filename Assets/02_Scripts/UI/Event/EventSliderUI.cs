using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventSliderUI : MonoBehaviour
{
    [Header("Prop")]
    [SerializeField] private Prop prop;

    [Header("Slider Bar")]
    [SerializeField] private Slider runeHpBar;


    private void Start()
    {
        ResetBars();
        prop.StatBarChange += ResetBars;
    }

    private void OnDestroy()
    {
        if (prop != null) 
            prop.StatBarChange -= ResetBars;
    }

    private void ResetBars()
    {
        Vector3 offset = new Vector3(0, -1, 0);
        Vector3 position = prop.transform.position + offset;

        if (prop == null)
            return;

        runeHpBar.transform.position += position;
        SetBar(runeHpBar, prop.stats[StatType.Hp], prop.maxStats[MaxStatType.MaxHp]);
    }

    private void SetBar(Slider bar, float current, float max)
    {
        if (bar == null)
            return;

        bar.maxValue = max;
        bar.value = current;
    }




}
