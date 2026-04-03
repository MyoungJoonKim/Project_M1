using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Scene_Manager : MonoBehaviour
{
    public Scene scene;
    public Scene nextScene;

    public void ChangeScene(Scene next, bool loading = false)
    {
        if (scene == next)
            return;

        if (loading)
        {
            nextScene = next;
            scene = Scene.LOADING;
            SceneManager.LoadScene((int)Scene.LOADING);
            return;
        }

        scene = next;
        SceneManager.LoadScene((int)next);

        switch (next)
        {
            case Scene.TITLE:
                break;
            case Scene.LOADING:
                break;
            case Scene.LOBBY:
                break;
            case Scene.BATTLE:
                break;
            case Scene.END:
                break;
        }
    }
}
