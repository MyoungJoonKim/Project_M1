using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Title : MonoBehaviour
{
    public GameObject option;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonGamePlay()
    {
        Shared.Scene_Manager.ChangeScene(SCENE.LOBBY, true);
    }
    public void OnButtonOption()
    {
        option.SetActive(true);
    }
    public void OnButtonGameQuit()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
            Application.Quit();
    }
}
