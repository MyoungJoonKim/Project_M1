using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Scene_Manager : MonoBehaviour
{
    public SceneType scene;
    public SceneType nextScene;

    public void ChangeScene(SceneType next, bool loading = false)
    {
        if (scene == next)
            return;

        if (loading)
        {
            nextScene = next;
            scene = SceneType.LOADING;
            SceneManager.LoadScene((int)SceneType.LOADING);
            return;
        }

        scene = next;
        SceneManager.LoadScene((int)next);

        switch (next)
        {
            case SceneType.TITLE:
                break;
            case SceneType.LOADING:
                break;
            case SceneType.LOBBY:
                break;
            case SceneType.BATTLE:
                break;
            case SceneType.END:
                break;
        }
    }
}
