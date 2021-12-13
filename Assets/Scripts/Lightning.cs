using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public GameObject lightningPrefab;
    public Pigeon.FrameAnimation[] lightningAnimations;

    public MoveBounds bounds;
    public Vector2 lightningScale;

    public float lightningDifficulty;
    [Range(0f, 1f)] public float lightningChance;

    void FixedUpdate()
    {
        if (DifficultyManager.instance.difficulty > lightningDifficulty && Random.value <= lightningChance)
        {
            float scale = Random.Range(lightningScale.x, lightningScale.y);
            Vector3 pos = bounds.RandomPointWorld();
            pos.z = 30f;
            Pigeon.Animator lightning = Instantiate(lightningPrefab, pos, Quaternion.identity).GetComponent<Pigeon.Animator>();
            lightning.transform.localScale = new Vector3(scale, scale, scale);
            lightning.Play(lightningAnimations[Random.Range(0, lightningAnimations.Length)]);
        }
    }
}