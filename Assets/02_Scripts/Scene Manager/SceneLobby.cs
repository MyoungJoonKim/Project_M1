using UnityEngine;

public class SceneLobby : MonoBehaviour
{
    public void OnButtonGamePlay()
    {
        CoreService.sceneLoadManager.ChangeScene(SceneType.BATTLE, true);
    }
}
