using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    TITLE,
    LOADING,
    LOBBY,
    BATTLE,
    END
}

public class SceneLoadManager : MonoBehaviour
{
    public SceneType scene;
    public SceneType nextScene;

    private void Awake()
    {
        if (CoreService.sceneLoadManager != null && CoreService.sceneLoadManager != this)
        {
            Destroy(gameObject);
            return;
        }

        CoreService.sceneLoadManager = this;
        DontDestroyOnLoad(gameObject);
    }

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
