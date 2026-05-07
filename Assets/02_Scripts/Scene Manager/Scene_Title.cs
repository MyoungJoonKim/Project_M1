using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Title : MonoBehaviour
{
    public void OnButtonGamePlay()
    {
        Shared.TMP_TextBlink.textBlink = false;
        Shared.scene_Manager.ChangeScene(SceneType.LOBBY, true);
    }
}
