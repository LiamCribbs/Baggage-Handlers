using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleIn : MonoBehaviour
{
    public float speed;

    void Start()
    {
        StartCoroutine(Scale());
    }

    IEnumerator Scale()
    {
        float targetScale = transform.localScale.x;
        float time = 0f;

        while (time < 1f)
        {
            time += speed * Time.deltaTime;
            if (time > 1f)
            {
                time = 1f;
            }

            float scale = targetScale * Pigeon.EaseFunctions.EaseOutCubic(time);
            transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }
    }
}