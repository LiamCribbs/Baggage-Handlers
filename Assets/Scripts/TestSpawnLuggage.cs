using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnLuggage : MonoBehaviour
{
    public GameObject luggagePrefab;

    public float spawnRadius;
    public float spawnDelay = 1f;
    public Vector2 scale;
    public float speed;
    public float spin;

    void Awake()
    {
        StartCoroutine(SpawnLuggage());
    }

    IEnumerator SpawnLuggage()
    {
        while (true)
        {
            Transform luggage = Instantiate(luggagePrefab, transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))).transform;
            luggage.localScale *= Random.Range(scale.x, scale.y);
            var rigidbody = luggage.GetComponent<Rigidbody2D>();
            rigidbody.velocity = new Vector2(-speed, 0f);
            rigidbody.angularVelocity = Random.value * spin;
            luggage.GetComponent<SpriteRenderer>().color = Random.ColorHSV(200f / 360f, 290f / 360f, 0.6f, 1f, 1f, 1f);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}