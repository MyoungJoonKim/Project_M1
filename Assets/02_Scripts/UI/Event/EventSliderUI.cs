using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EventSliderUI : MonoBehaviour
{
    [Header("Prop")]
    [SerializeField] private Prop prop;

    [Header("Slider Bar")]
    [SerializeField] private Slider runeHpBar;

    [Header("Position")]
    [SerializeField] private float offset = -2.5f;

    private void Start()
    {
        if (prop == null)
            prop = GetComponent<Prop>();

        if (prop == null)
            return;

        ResetBars();
        prop.StatBarChange += ResetBars;
        StartCoroutine(BarPositionUpdate());
    }

    private void OnDestroy()
    {
        if (prop != null) 
            prop.StatBarChange -= ResetBars;
    }

    private IEnumerator BarPositionUpdate()
    {
        while (true)
        {
            if (prop == null)
                yield break;

            if (Camera.main != null && runeHpBar != null)
            {
                Vector3 worldPos = prop.transform.position + new Vector3(0f, offset, 0f);

                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

                runeHpBar.transform.position = screenPos;
            }
            yield return null;
        }
    }

    private void ResetBars()
    {
        if (prop == null || runeHpBar == null)
            return;

        SetBar(runeHpBar, prop.stats[StatType.Hp], prop.maxStats[MaxStatType.MaxHp]);
    }

    private void SetBar(Slider bar, float current, float max)
    {
        if (bar == null)
            return;

        bar.maxValue = max;
        bar.value = current;
    }

    public void SetActiveBar(bool value)
    {
        if (runeHpBar != null) 
            runeHpBar.gameObject.SetActive(value);
    }


}
