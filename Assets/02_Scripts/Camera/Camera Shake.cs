using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 cameraPosition;
    public Vector3 ShakeOffset => cameraPosition;

    private Coroutine shakeCoroutine;

    public void Shake(float duration, float strength)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, strength));
    }

    private IEnumerator ShakeCoroutine(float duration, float strength)
    {
        float timer = 0f;

        while (timer < duration)
        {
            float x = Random.Range(-strength, strength);
            float y = Random.Range(-strength, strength);

            cameraPosition = new Vector3(x, y, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        cameraPosition = Vector3.zero;
        shakeCoroutine = null;
    }
}
