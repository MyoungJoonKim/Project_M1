using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Texts")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private TMP_Text killRecordText;

    [Header("UI Panel")]
    [SerializeField] private GameObject gameOverUI;

    [Header("UserExp Slider")]
    [SerializeField] private Slider userExp;

    private Coroutine levelTextCoroutine;
    private Coroutine timeTextCoroutine;
    private Coroutine roundTextCoroutine;

    public string TimeText => timeText.text;


    private void Start()
    {
        gameOverUI.SetActive(false);
        FindPlayer();
        StartBattleUI();
    }

    private IEnumerator UpdateLevelUI()
    {
        while (true)
        {
            if (Shared.battleManager == null || player == null)
            {
                yield return null;
                continue;
            }

            if (levelText != null)
            {
                float currentLevel = player.GetStat(StatType.Level);
                levelText.text = $"Lv. {currentLevel}";
            }
            yield return null;
        }
    }

    private IEnumerator UpdateTimeUI()
    {
        while (true)
        {
            if (Shared.battleManager == null)
            {
                yield return null;
                continue;
            }

            if (!Shared.battleManager.isBattlePlaying)
            {
                yield return null;
                continue;
            }

            if (timeText == null)
                yield break;

            float time = Shared.battleManager.GameTime;

            int minute = Mathf.FloorToInt(time / 60f);
            int second = Mathf.FloorToInt(time % 60f);

            timeText.text = $"{minute:00}:{second:00}";

            yield return null;
        }
    }

    private IEnumerator UpdateRoundUI()
    {
        while (true)
        {
            if (Shared.battleManager == null)
            {
                yield return null;
                continue;
            }

            if (!Shared.battleManager.isBattlePlaying)
            {
                yield return null;
                continue;
            }

            if (roundText == null)
                yield break;

            if (Shared.spawnManager != null)
                roundText.text = $"Round {Shared.spawnManager.CurrentRoundNumber}";
            else
                roundText.text = "Round -";

            yield return null;
        }
    }

    public void StartBattleUI()
    {
        if (levelTextCoroutine != null)
            StopCoroutine(levelTextCoroutine);

        levelTextCoroutine = StartCoroutine(UpdateLevelUI());

        if (timeTextCoroutine != null)
            StopCoroutine(timeTextCoroutine);

        timeTextCoroutine = StartCoroutine(UpdateTimeUI());

        if (roundTextCoroutine != null)
            StopCoroutine(roundTextCoroutine);

        roundTextCoroutine = StartCoroutine(UpdateRoundUI());
    }

    public void StopBattleUI()
    {
        if (timeTextCoroutine != null)
        {
            StopCoroutine(timeTextCoroutine);
            timeTextCoroutine = null;
        }

        if (roundTextCoroutine != null)
        {
            StopCoroutine(roundTextCoroutine);
            roundTextCoroutine = null;
        }
    }

    public IEnumerator GameOverUI()
    {
        yield return new WaitForSeconds(3f);
        gameOverUI.SetActive(true);
        killRecordText.text = $"{Shared.battleManager.killRecord}";
        survivalTimeText.text = timeText.text;

    }

    private void FindPlayer()
    {
        if (player != null)
            return;

        if (Shared.battleManager != null && Shared.battleManager.player != null)
        {
            player = Shared.battleManager.player;
            return;
        }

        player = FindObjectOfType<Player>();
    }
}
