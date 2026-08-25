using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject gameOverUI;

    [Header("UserExp Slider")]
    [SerializeField] private Slider userExpSlider;

    [Header("Result Texts")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private TMP_Text killRecordText;

    [Header("Reward Texts")]
    [SerializeField] private TMP_Text addGoldText;
    [SerializeField] private TMP_Text addExpText;

    [Header("User Texts")]
    [SerializeField] private TMP_Text userLevelText;
    [SerializeField] private TMP_Text userExpText;

    [Header("Button")]
    [SerializeField] private Button endButton;

    private BattleManager battleManager;

    private int rewardGold;
    private int rewardExp;
    private bool rewardApplied;
    private Coroutine rewardCoroutine;

    private void Start()
    {
        battleManager = GetComponent<BattleManager>();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public IEnumerator GameOverUI(bool isVictory)
    {
        yield return new WaitForSecondsRealtime(3f);
        OpenUI(isVictory);
    }

    private void OpenUI(bool isVictory)
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        if (battleManager == null)
            return;

        if (Shared.userManager == null)
            return;

        rewardApplied = false;

        rewardGold = battleManager.GetRewardGold();
        rewardExp = battleManager.GetRewardUserExp();

        BattleUI battleUI = FindAnyObjectByType<BattleUI>();

        if (titleText != null && !isVictory)
            titleText.text = "사망\n\n생존시간";
        else if (titleText != null && isVictory)
            titleText.text = "승리\n\n생존시간";

        if (survivalTimeText != null && battleUI != null)
            survivalTimeText.text = battleUI.TimeText;

        if (killRecordText != null)
            killRecordText.text = $"{battleManager.killRecord}";

        if (addGoldText != null)
            addGoldText.text = $"{rewardGold}";

        if (addExpText != null)
            addExpText.text = $"{rewardExp}";


        if (rewardCoroutine != null)
            StopCoroutine(rewardCoroutine);

        rewardCoroutine = StartCoroutine(ExpBarUpdate(userExpSlider, rewardExp));
    }

    private IEnumerator ExpBarUpdate(Slider bar, float addExp)
    {
        if (bar == null)
            yield break;

        UserData data = Shared.userManager.userData;

        float exp = data.userExp;
        float maxExp = data.userMaxExp;
        int level = data.userLevel;

        bar.wholeNumbers = false;
        bar.minValue = 0;
        bar.maxValue = maxExp;
        bar.value = exp;

        RefreshUserExpUI(level, exp, maxExp);

        while (addExp > 0)
        {
            float addValue = 0.2f;

            if (addExp < addValue)
                addValue = addExp;

            exp += addValue;
            addExp -= addValue;

            if (exp >= maxExp)
            {
                exp -= maxExp;
                level++;
                maxExp *= 1.5f;

                bar.maxValue = maxExp;
                bar.value = 0f;
            }

            bar.maxValue = maxExp;
            bar.value = exp;

            RefreshUserExpUI(level, exp, maxExp);

            yield return new WaitForSecondsRealtime(0.001f);
        }

        bar.maxValue = maxExp;
        bar.value = exp;
        RefreshUserExpUI(level, exp, maxExp);

        if (endButton != null)
            endButton.interactable = true;

        rewardCoroutine = null;
    }

    private void RefreshUserExpUI(int level, float exp, float maxExp)
    {
        if (userLevelText != null)
            userLevelText.text = $"Lv. {level}";

        if (userExpText != null)
            userExpText.text = $"{Mathf.FloorToInt(exp)} / {Mathf.FloorToInt(maxExp)}";
    }

    private void ApplyReward()
    {
        if (rewardApplied)
            return;

        rewardApplied = true;

        if (Shared.userManager == null)
            return;

        Shared.userManager.AddGold(rewardGold);
        Shared.userManager.AddUserExp(rewardExp);
    }

    public void OnClickEndButton()
    {
        Time.timeScale = 1f;

        ApplyReward();

        if (Shared.sceneLoadManager != null)
            Shared.sceneLoadManager.ChangeScene(SceneType.LOBBY, false);
    }
}