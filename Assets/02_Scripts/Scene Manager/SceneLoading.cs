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
        StartCoroutine(LoadingBarUpdate(loadingBar,3f));
        StartCoroutine(BackgroundUpdate(3f));
    }

    private IEnumerator LoadingBarUpdate(Slider bar, float timer)
    {
        if (bar == null)
            yield break;

        bar.value = bar.minValue;
        bar.maxValue = timer;

        while (true)
        {
            bar.value += 0.02f;
            yield return new WaitForSeconds(0.01f);
            if (bar.value == timer)
                break;
        }
        SceneLoadManager.Instance.ChangeScene(SceneLoadManager.Instance.nextScene, false);
    }

    private IEnumerator BackgroundUpdate(float timer)
    {
        while (true)
        {
            RandomUpdateText();
            yield return new WaitForSeconds(1f);

            if (loadingBar.value == timer)
                break;
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
