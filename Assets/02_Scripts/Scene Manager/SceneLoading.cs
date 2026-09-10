using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoading : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text gameTipText;


    private List<string> texts = new List<string>();


    private void Start()
    {
        StartCoroutine(LoadBattleScene());
    }

    private IEnumerator LoadBattleScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync($"{SceneLoadManager.Instance.nextScene}");

        asyncOperation.allowSceneActivation = false;    

        if (loadingBar == null)
            yield break;

        loadingBar.minValue = 0f;
        loadingBar.maxValue = 1f;
        loadingBar.value = 0f;

        StartCoroutine(BackgroundTextUpdate());

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            loadingBar.value = progress;

            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(3f);

                loadingBar.maxValue = 1f;
                
                StopCoroutine(BackgroundTextUpdate());

                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator BackgroundTextUpdate()
    {
        while (true)
        {
            RandomUpdateText();
            yield return new WaitForSeconds(2f);
        }
    }

    private void RandomUpdateText()
    {
        texts.Add("1.룬기둥 파괴에 실패하면 몬스터가 몰려옵니다.");
        texts.Add("2.패시브 스킬은 6레벨부터 선택 가능합니다.");
        texts.Add("3.룬기둥을 파괴하면 강력한 스킬을 사용합니다.");
        texts.Add("4.게임 옵션에서 사운드 음량 조절이 가능합니다.");
        texts.Add("5.게임 옵션에서 언어를 변경할 수 있습니다.");

        int rand = Random.Range(0, texts.Count);
        gameTipText.text = texts[rand];
    }
}
