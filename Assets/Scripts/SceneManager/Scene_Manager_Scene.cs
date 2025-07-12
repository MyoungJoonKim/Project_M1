using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Scene_Manager : MonoBehaviour
{
    public SCENE Scene;

    public void ChangeScene(SCENE next, bool loading = false)
    {
        if (Scene == next)
            return;

        Scene = next;

        if (loading)
        {
            SceneManager.LoadScene((int)SCENE.LOADING);
            return;
        }

        SceneManager.LoadScene((int)Scene);


        switch (next) // 서버 데이터를 초기화(리셋)하기 위해 필요함.
        {
            case SCENE.TITLE:
                break;
            case SCENE.LOBBY:
                break;
            case SCENE.BATTLE:
                break;
            case SCENE.LOADING:
                break;
            case SCENE.END:
                break;

        }
    }
}
