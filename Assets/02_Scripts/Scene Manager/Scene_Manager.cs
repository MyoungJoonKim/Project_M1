using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Scene_Manager : MonoBehaviour
{
    private void Awake()
    {
        if (Shared.scene_Manager = null)
        {
            Shared.scene_Manager = this;
            DontDestroyOnLoad(this);
        }
    }
}
