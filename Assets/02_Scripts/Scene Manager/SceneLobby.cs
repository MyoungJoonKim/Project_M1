using UnityEngine;

public class SceneLobby : MonoBehaviour
{
    public void OnButtonGamePlay()
    {
        Shared.sceneLoadManager.ChangeScene(SceneType.BATTLE, true);
    }
}
