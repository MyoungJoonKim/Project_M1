using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Lobby : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadingTime()
    {
        yield return new WaitForSeconds(5f);
    }

    
}
