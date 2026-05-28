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
    [SerializeField] private Slider userExp;

    [Header("Texts")]
    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private TMP_Text killRecordText;

    private BattleUI battleUI;
    private void Start()
    {
        gameOverUI.SetActive(false);
    }

    public IEnumerator GameOverUI()
    {
        yield return new WaitForSeconds(3f);
        gameOverUI.SetActive(true);
        killRecordText.text = $"{Shared.battleManager.killRecord}";
        survivalTimeText.text = battleUI.TimeText;

    }
}
