using UnityEngine;

public class SceneLobby : MonoBehaviour
{
    public void OnButtonGamePlay()
    {
        SceneLoadManager.Instance.ChangeScene(SceneType.BATTLE, true);
    }
}
