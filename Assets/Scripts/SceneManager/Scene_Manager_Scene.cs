using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Scene_Manager : MonoBehaviour
{
    public SCENE Scene;
    public SCENE NextScene;

    public void ChangeScene(SCENE next, bool loading = false)
    {
        if (Scene == next)
            return;

        if (loading)
        {
            NextScene = next;
            Scene = SCENE.LOADING;
            SceneManager.LoadScene((int)SCENE.LOADING);
            return;
        }

        Scene = next;
        SceneManager.LoadScene((int)next);


        switch (next) // ���� �����͸� �ʱ�ȭ(����)�ϱ� ���� �ʿ���.
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
