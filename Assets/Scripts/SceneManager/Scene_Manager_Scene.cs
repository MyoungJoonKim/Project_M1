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

        SceneManager.LoadScene((int)next);

        if (loading)
        {
            SceneManager.LoadScene((int)SCENE.LOADING);
            return;
        }

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
