using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score;
    public TextMeshProUGUI scoreText;
    public float textBulgeSpeedIn;
    public float textBulgeSpeedOut;
    public float textBulgeScale;
    Coroutine bulgeTextCoroutine;

    [Space(20)]
    public Graphic healthBarFill;
    public Graphic healthBarDamageFill;
    public float removeHealthSpeed;
    Coroutine removeHealthCoroutine;

    [Space(20)]
    public TextMeshProUGUI harpoonsText;

    void Awake()
    {
        instance = this;
    }

    public static void AddScore(int amount)
    {
        instance.score += amount;
        instance.scoreText.text = instance.score.ToString();

        if (instance.bulgeTextCoroutine != null)
        {
            instance.StopCoroutine(instance.bulgeTextCoroutine);
        }
        instance.bulgeTextCoroutine = instance.StartCoroutine(instance.BulgeText(instance.scoreText));
    }

    IEnumerator BulgeText(TextMeshProUGUI text)
    {
        float initialScale = text.transform.localScale.x;
        float time = 0f;

        while (time < 1f)
        {
            time += textBulgeSpeedIn * Time.deltaTime;
            if (time > 1f)
            {
                time = 1f;
            }

            float scale = Mathf.LerpUnclamped(initialScale, textBulgeScale, Pigeon.EaseFunctions.EaseOutQuartic(time));
            text.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }

        initialScale = text.transform.localScale.x;
        time = 0f;

        while (time < 1f)
        {
            time += textBulgeSpeedOut * Time.deltaTime;
            if (time > 1f)
            {
                time = 1f;
            }

            float scale = Mathf.LerpUnclamped(initialScale, 1f, Pigeon.EaseFunctions.EaseOutElastic(time));
            text.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }

        bulgeTextCoroutine = null;
    }

    public void UpdateHealthBar(PilotControls player)
    {
        float health = (float) player.Health / player.maxHealth;
        Vector2 oldHealth = healthBarFill.rectTransform.anchorMax;

        healthBarDamageFill.rectTransform.anchorMin = new Vector2(health, 0f);
        healthBarDamageFill.rectTransform.anchorMax = oldHealth;
        healthBarDamageFill.rectTransform.offsetMin = Vector2.zero;
        healthBarDamageFill.rectTransform.offsetMax = Vector2.zero;

        healthBarFill.rectTransform.anchorMax = new Vector2(health, 1f);
        healthBarFill.rectTransform.offsetMax = Vector2.zero;

        if (removeHealthCoroutine != null)
        {
            StopCoroutine(removeHealthCoroutine);
        }

        removeHealthCoroutine = StartCoroutine(RemoveHealthCoroutine(health, oldHealth.x));
    }

    IEnumerator RemoveHealthCoroutine(float min, float max)
    {
        float time = 0f;

        while (time < 1f)
        {
            time += removeHealthSpeed * Time.deltaTime;
            if (time > 1f)
            {
                time = 1f;
            }

            float t = Pigeon.EaseFunctions.EaseInOutQuartic(time);

            healthBarDamageFill.rectTransform.anchorMax = new Vector2(Mathf.LerpUnclamped(max, min, t), 1f);
            healthBarDamageFill.rectTransform.offsetMin = Vector2.zero;
            healthBarDamageFill.rectTransform.offsetMax = Vector2.zero;

            yield return null;
        }

        removeHealthCoroutine = null;
    }
}