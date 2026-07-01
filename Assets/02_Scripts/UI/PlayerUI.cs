using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Slider Bars")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider expBar;


    private void Start()
    {
        if (player == null)
            player = FindAnyObjectByType<Player>();

        if (player == null)
        {
            enabled = false;
            return;
        }

        ResetBars();
        player.StatBarChange += ResetBars;

        StartCoroutine(BarPositionUpdate());
    }

    private IEnumerator BarPositionUpdate()
    {
        while (true)
        {
            if (player == null)
                yield break;

            if (Camera.main != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
                hpBar.transform.position = screenPos + new Vector3(0,- 80, 0);
                yield return null;
            }
        } 
    }

    private void OnDestroy()
    {
        if (player != null) 
            player.StatBarChange -= ResetBars;
    }

    private void ResetBars()
    {
        if (player == null)
            return;

        SetBar(hpBar, player.stats[StatType.Hp], player.maxStats[MaxStatType.MaxHp]);
        SetBar(expBar, player.stats[StatType.Exp], player.maxStats[MaxStatType.MaxExp]);
    }

    private void SetBar(Slider bar, float current, float max)
    {
        if (bar == null)
            return;

        bar.maxValue = max;
        bar.value = current;
    }

    
}
