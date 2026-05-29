using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private TMP_Text killRecordText;

    [Header("Reward Texts")]
    [SerializeField] private TMP_Text addGoldText;
    [SerializeField] private TMP_Text addExpText;

    [Header("User Texts")]
    [SerializeField] private TMP_Text userLevelText;
    [SerializeField] private TMP_Text userExpText;


    private int rewardGold;
    private int rewardExp;
    private bool rewardApplied;
    private Coroutine rewardCoroutine;


    private void Start()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public IEnumerator GameOverUI()
    {
        BattleUI battleUI = FindAnyObjectByType<BattleUI>();
        yield return new WaitForSeconds(3f);
        gameOverUI.SetActive(true);
        killRecordText.text = $"{Shared.battleManager.killRecord}";
        survivalTimeText.text = battleUI.TimeText;

    }
}
