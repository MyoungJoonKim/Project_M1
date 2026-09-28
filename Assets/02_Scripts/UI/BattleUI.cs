using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Texts")]
    [SerializeField] private TMP_Text[] levelText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text waveText;

    [Header("Slider")]
    [SerializeField] private Slider waveSlider;

    [Header("UIs")]
    [SerializeField] private PauseUI pauseUI;

    [Header("Manager")]
    [SerializeField] private SpawnManager spawnManager;

    private OpenUI openUI;
    private CloseUI closeUI;

    private Coroutine levelTextCoroutine;
    private Coroutine timeTextCoroutine;
    private Coroutine waveTextCoroutine;
    private Coroutine waveSliderCoroutine;

    public string TimeText => timeText.text;

    private void Start()
    {
        openUI = GetComponent<OpenUI>();
        closeUI = GetComponent<CloseUI>();

        FindPlayer();
        StartBattleUI();
    }

    private IEnumerator UpdateLevelUI()
    {
        while (true)
        {
            if (BattleManager.Instance == null || player == null)
            {
                yield return null;
                continue;
            }

            if (levelText != null)
            {
                float currentLevel = player.GetStat(StatType.Level);
                levelText[0].text = $"Lv. {currentLevel}";
                levelText[1].text = $"Lv. {currentLevel}";
            }
            yield return null;
        }
    }

    private IEnumerator UpdateTimeUI()
    {
        while (true)
        {
            if (BattleManager.Instance == null)
            {
                yield return null;
                continue;
            }

            if (!BattleManager.Instance.isBattlePlaying)
            {
                yield return null;
                continue;
            }

            if (timeText == null)
                yield break;

            float time = BattleManager.Instance.GameTime;

            int minute = Mathf.FloorToInt(time / 60f);
            int second = Mathf.FloorToInt(time % 60f);

            timeText.text = $"{minute:00}:{second:00}";

            yield return null;
        }
    }

    private IEnumerator UpdateWaveUI()
    {
        while (true)
        {
            if (BattleManager.Instance == null)
            {
                yield return null;
                continue;
            }

            if (!BattleManager.Instance.isBattlePlaying)
            {
                yield return null;
                continue;
            }

            if (spawnManager != null)
            {
                int currentWave = Mathf.Clamp(spawnManager.CurrentWaveIndex + 1, 1, spawnManager.MaxWaveCount);

                waveText.text = $"{currentWave} / {spawnManager.MaxWaveCount} ";
            }
            else
                waveText.text = "- / -";

            yield return null;
        }
    }

    private IEnumerator UpdateWaveSlider()
    {
        int maxCount = spawnManager.MaxRoundCount * spawnManager.MaxWaveCount;

        waveSlider.maxValue = maxCount;
        waveSlider.minValue = 0;
        waveSlider.value = 0;

        while (true)
        {
            if (spawnManager.CurrentRoundIndex < 1)
            {
                waveSlider.value = spawnManager.CurrentWaveIndex + 1;
            }
            else
            {
                waveSlider.value = spawnManager.CurrentWaveIndex + 11;
            }

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

        if (waveTextCoroutine != null)
            StopCoroutine(waveTextCoroutine);

        waveTextCoroutine = StartCoroutine(UpdateWaveUI());

        if (waveSliderCoroutine != null)
            StopCoroutine(waveSliderCoroutine);

        waveSliderCoroutine = StartCoroutine(UpdateWaveSlider());
    }

    public void StopBattleUI()
    {
        if (timeTextCoroutine != null)
        {
            StopCoroutine(timeTextCoroutine);
            timeTextCoroutine = null;
        }

        if (waveTextCoroutine != null)
        {
            StopCoroutine(waveTextCoroutine);
            waveTextCoroutine = null;
        }

        if (waveSliderCoroutine != null)
        {
            StopCoroutine(waveSliderCoroutine);
            waveSliderCoroutine = null;
        }
    }

    private void FindPlayer()
    {
        if (player != null)
            return;

        if (BattleManager.Instance != null && BattleManager.Instance.player != null)
        {
            player = BattleManager.Instance.player;
            return;
        }

        player = FindObjectOfType<Player>();
    }

    public void OnClickPauseButton()
    {
        if (openUI != null) 
            openUI.OnClickOpenUI();

        if (pauseUI != null)
            pauseUI.Open();
    }

    public void OnClickPlayButton()
    {
        if (closeUI != null)
            closeUI.OnClickCloseUI();

        if (pauseUI != null)
            pauseUI.Close();
    }
}
