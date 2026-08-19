using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [Header("TMP")]
    [SerializeField] private TextMeshPro textMeshPro;

    [Header("Text Effect Value")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float lifeTime = 1f;

    private DamageTextManager manager;
    private Coroutine effectCoroutine;

    public void SetManager(DamageTextManager manager)
    {
        this.manager = manager;
    }

    public void SetUp(float damage)
    {
        if (textMeshPro != null)
            textMeshPro.text = damage.ToString();

        StopEffect();
        effectCoroutine = StartCoroutine(TextEffect());
    }

    private IEnumerator TextEffect()
    {
        float timer = lifeTime;

        while (timer > 0f)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }

        effectCoroutine = null;

        if (manager != null)
            manager.Release(this);
        else
            gameObject.SetActive(false);
    }
    public void StopEffect()
    {
        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
            effectCoroutine = null;
        }
    }
}
