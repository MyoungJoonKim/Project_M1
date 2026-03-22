using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public JoyStick JoyStick;
    public float speed = 3f;
    void Update()
    {
        Vector2 input = JoyStick.Input;

        Vector2 move = new Vector2 (input.x, input.y);
        transform.Translate (move * speed * Time.deltaTime);
    }
}
