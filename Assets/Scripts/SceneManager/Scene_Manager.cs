using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Scene_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (Shared.Scene_Manager == null)
        {
            Shared.Scene_Manager = this;

            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
