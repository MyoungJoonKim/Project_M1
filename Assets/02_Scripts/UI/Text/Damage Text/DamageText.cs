using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float lifeTime = 1f;


    private void Awake()
    {
        if (Shared.damageText == null)
        {
            Shared.damageText = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetUp(float damage)
    {
        textMeshPro.text = damage.ToString();

        StartCoroutine(TextEffect());
    }

    IEnumerator TextEffect()
    {
        float timer = lifeTime;

        while (timer > 0)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            timer -= Time.deltaTime;
            float time = 1f - (timer /  lifeTime);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
