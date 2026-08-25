using System.Collections;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Texts")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text roundText;

    [Header("UIs")]
    [SerializeField] private PauseUI pauseUI;

    [Header("Manager")]
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private BattleManager battleManager;

    private Coroutine levelTextCoroutine;
    private Coroutine timeTextCoroutine;
    private Coroutine roundTextCoroutine;

    public string TimeText => timeText.text;

    private void Start()
    {
        FindPlayer();
        StartBattleUI();
    }

    private IEnumerator UpdateLevelUI()
    {
        while (true)
        {
            if (battleManager == null || player == null)
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
            if (battleManager == null)
            {
                yield return null;
                continue;
            }

            if (!battleManager.isBattlePlaying)
            {
                yield return null;
                continue;
            }

            if (timeText == null)
                yield break;

            float time = battleManager.GameTime;

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
            if (battleManager == null)
            {
                yield return null;
                continue;
            }

            if (!battleManager.isBattlePlaying)
            {
                yield return null;
                continue;
            }

            if (roundText == null)
                yield break;

            if (spawnManager != null)
                roundText.text = $"R {spawnManager.CurrentRoundNumber} / W {spawnManager.CurrentWaveIndex + 1} ";
            else
                roundText.text = "Round - / Wave -";

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

    private void FindPlayer()
    {
        if (player != null)
            return;

        if (battleManager != null && battleManager.player != null)
        {
            player = battleManager.player;
            return;
        }

        player = FindObjectOfType<Player>();
    }

    public void OnClickPauseButton()
    {
        if (pauseUI != null)
            pauseUI.Open();
    }
}
