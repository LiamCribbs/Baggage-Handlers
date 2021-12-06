using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployDebris : MonoBehaviour
{
    public const int DebrisLayer = 9;

    public GameObject debrisPrefab;
    public Vector2 waitTime;
    public AnimationCurve waitCurve;

    public Vector2 minVelocity, maxVelocity;

    public MoveBounds spawnBounds;

    void Start()
    {
        //screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(DebrisWave());
    }


    private void SpawnDebris()
    {
        GameObject debris = Instantiate(debrisPrefab, spawnBounds.RandomPointWorld(), Quaternion.Euler(0f, 0f, Random.value * 360f));
        debris.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(minVelocity.x, maxVelocity.x), Random.Range(minVelocity.y, maxVelocity.y));
        DestroyTrigger.instance.destroyableObjects.Add(debris.transform);
        //d.transform.position = new Vector2(screenBounds.x, Random.Range(-screenBounds.y, screenBounds.y));
    }

    IEnumerator DebrisWave()
    {
        while (true)
        {
            float time = 0f;
            float duration = waitCurve.Evaluate(Random.value) * (waitTime.y - waitTime.x) + waitTime.x;

            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }

            SpawnDebris();
        }
    }
}
