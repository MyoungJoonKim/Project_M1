using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Lobby : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonBattle()
    {
        Shared.Scene_Manager.ChangeScene(SCENE.BATTLE, true);
    }

}
