using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform Player;

    [Header("Move Limit")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Camera Offset")]
    [SerializeField] private CameraShake cameraShake;

    [Header("Camera Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -100f);


    private void LateUpdate()
    {
        if (Player == null)
            return;
        
        Vector3 targetPosition = Player.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        if (cameraShake != null)
            targetPosition += cameraShake.ShakeOffset;

        transform.position = targetPosition;    
    }
}
