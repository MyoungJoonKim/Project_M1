using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public JoyStick JoyStick;
    private float speed = 3f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 input = JoyStick.Input;
        Vector2 move = input.normalized;
        rb.velocity = move * speed;
    }
}
