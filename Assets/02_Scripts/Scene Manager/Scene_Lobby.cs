using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_Lobby : MonoBehaviour
{
    public void OnButtonGamePlay()
    {
        Shared.scene_Manager.ChangeScene(SceneType.BATTLE, true);
    }
}
