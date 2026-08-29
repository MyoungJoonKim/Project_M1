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
    public static SceneLoadManager Instance;

    public SceneType scene;
    public SceneType nextScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
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
