using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 position = new Vector3(0, 5f, -10f);

    // Start is called before the first frame update
    void Start()
    {
    }
    void Update()
    {
        Move();
    }


    void Move()
    {
        this.gameObject.transform.position = player.transform.position + position;
    }
}
