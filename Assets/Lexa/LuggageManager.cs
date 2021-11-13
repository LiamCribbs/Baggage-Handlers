using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LuggageManager : MonoBehaviour
{

    //objects to spawn
    public GameObject bags;

    //toss times
    public float minTime = 1;
    public float maxTime = 5;

    //internal times
    private float time;
    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomTime();
        time = minTime;
    }

    private void FixedUpdate()
    {
        //counting up
        time += Time.deltaTime;

        //check for spawn
        if(time >= spawnTime)
        {
            SpawnObject();
            SetRandomTime();
        }
    }

    //spawn object
    void SpawnObject()
    {
        time = minTime;
        Instantiate(bags, transform.position, bags.transform.rotation);
    }

    void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
