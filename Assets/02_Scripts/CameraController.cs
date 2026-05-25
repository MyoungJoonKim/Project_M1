using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform Player;

    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0, 0, -100f);

    [Header("Move Limit")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void LateUpdate()
    {
        if (Player == null) 
            return;

        Vector3 targetPosition = Player.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        transform.position = targetPosition;    
    }
}
