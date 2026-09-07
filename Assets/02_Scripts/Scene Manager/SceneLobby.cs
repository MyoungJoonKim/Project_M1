using UnityEngine;

public class SceneLobby : MonoBehaviour
{
    public void OnButtonGamePlay()
    {
        SoundManager.Instance.PlayButtonClick();
        SceneLoadManager.Instance.ChangeScene(SceneType.BATTLE, true);
    }
}
