using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuggageManager : MonoBehaviour
{
    public LuggageList luggageList;

    public Transform luggageSpawnPosition;

    //toss times
    public float minSpawnTime = 1;
    public float maxSpawnTime = 5;

    void Start()
    {
        StartCoroutine(SpawnLuggageOverTime());
    }

    //private void FixedUpdate()
    //{
    //    //counting up
    //    time += Time.deltaTime;

    //    //check for spawn
    //    if(time >= spawnTime)
    //    {
    //        SpawnLuggage();
    //        SetRandomTime();
    //    }
    //}

    void SpawnLuggage()
    {
        // Get a random rotation snapped to 90 degrees and apply it to our rotation
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 3) * 90f) * luggageSpawnPosition.rotation;
        GameObject luggage = Instantiate(luggageList.GetRandom(), luggageSpawnPosition.position, rotation);
    }

    IEnumerator SpawnLuggageOverTime()
    {
        while (true)
        {
            // Pick random wait time
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            float time = 0f;

            // Wait
            while (time < waitTime)
            {
                time += Time.deltaTime;
                yield return null;
            }

            SpawnLuggage();
        }
    }
}