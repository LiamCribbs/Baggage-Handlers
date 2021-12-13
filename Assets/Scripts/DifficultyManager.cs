using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public float difficulty;

    public float difficultyIncreaseSpeed;

    [Space(20)]
    public float clearSkyDuration;
    public Transform skyRoot;
    public SpriteRenderer stormySky;
    public CloudPool cloudPool;
    public Sprite[] stormClouds;
    public float[] stormCloudTimes;
    bool[] addedStormClouds;

    [Space(20)]
    public DeployDebris deployDebris;
    public float debrisWaitMultiplier;
    public float maxDebrisWait;

    [Space(20)]
    public AIMovement luggagePlane;
    public MoveBounds[] planeMoveBounds;
    public float[] planeMoveBoundsTimes;

    void Awake()
    {
        instance = this;
        addedStormClouds = new bool[stormClouds.Length];
    }

    void Update()
    {
        difficulty += difficultyIncreaseSpeed * Time.deltaTime;

        stormySky.color = new Color(1f, 1f, 1f, difficulty - clearSkyDuration);

        float scale = CameraController.instance.camera.orthographicSize * 0.09583987f;
        skyRoot.localScale = new Vector3(scale, scale, scale);

        for (int i = 0; i < stormClouds.Length; i++)
        {
            if (!addedStormClouds[i] && difficulty > stormCloudTimes[i])
            {
                addedStormClouds[i] = true;
                cloudPool.cloudSprites.Add(stormClouds[i]);
                break;
            }
        }

        deployDebris.waitTime.y = Mathf.Clamp(debrisWaitMultiplier / (difficulty + 1f), 1f, maxDebrisWait);

        if (difficulty > planeMoveBoundsTimes[1])
        {
            luggagePlane.currentBounds = planeMoveBounds[1];
        }
        else if (difficulty > planeMoveBoundsTimes[0])
        {
            luggagePlane.currentBounds = planeMoveBounds[0];
        }
    }
}