using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPool : MonoBehaviour
{
    public int numClouds;
    public int maxGoingRight;
    public Vector2 moveSpeedMin;
    public Vector2 moveSpeedMax;
    public float minScale;
    public float maxScale;
    public AnimationCurve scaleCurve;

    public MoveBounds cloudBounds;

    public List<Sprite> cloudSprites;

    List<Cloud> clouds;

    class Cloud
    {
        public SpriteRenderer renderer;
        public Vector3 speed;
        public float halfWidth;
        public bool goingRight;
    }

    void Start()
    {
        clouds = new List<Cloud>(numClouds);

        for (int i = 0; i < numClouds; i++)
        {
            GameObject cloudGO = new GameObject("Cloud");
            cloudGO.transform.parent = transform;
            SpriteRenderer sprite = cloudGO.AddComponent<SpriteRenderer>();

            Cloud cloud = new Cloud()
            {
                renderer = sprite
            };

            clouds.Add(cloud);
            SetupCloud(cloud);
        }
    }

    void Update()
    {
        for (int i = 0; i < clouds.Count; i++)
        {
            Cloud cloud = clouds[i];

            Vector3 pos = cloud.renderer.transform.position + cloud.speed * Time.deltaTime;

            // Check left/right out of bounds
            if (cloud.goingRight ? pos.x - cloud.halfWidth > cloudBounds.OuterRightWorld : pos.x + cloud.halfWidth < cloudBounds.OuterLeftWorld)
            {
                SetupCloud(cloud);
                continue;
            }

            float bound = cloudBounds.OuterTopWorld;
            if (pos.y > bound)
            {
                pos.y = bound;
            }
            else
            {
                bound = cloudBounds.OuterBottomWorld;
                if (pos.y < bound)
                {
                    pos.y = bound;
                }
            }

            cloud.renderer.transform.position = pos;
        }
    }

    void SetupCloud(Cloud cloud)
    {
        cloud.renderer.sprite = cloudSprites[Random.Range(0, cloudSprites.Count)];

        int numGoingRight = 0;
        for (int i = 0; i < clouds.Count; i++)
        {
            if (clouds[i].goingRight)
            {
                numGoingRight++;
            }
        }

        cloud.speed = new Vector3(numGoingRight > maxGoingRight ? Random.Range(moveSpeedMin.x, -0.1f) : Random.Range(moveSpeedMin.x, moveSpeedMax.x), Random.Range(moveSpeedMin.y, moveSpeedMax.y));
        if (cloud.speed.x == 0f)
        {
            cloud.speed.x = 0.5f;
        }

        cloud.goingRight = cloud.speed.x > 0f;
        cloud.halfWidth = cloud.renderer.bounds.size.x * 0.5f;

        cloud.renderer.transform.position = new Vector3(cloud.goingRight ? cloudBounds.OuterLeftWorld - cloud.halfWidth : cloudBounds.OuterRightWorld + cloud.halfWidth,
            Random.Range(cloudBounds.OuterBottomWorld, cloudBounds.OuterTopWorld), transform.localPosition.z);

        float scale = scaleCurve.Evaluate(Random.Range(minScale, maxScale));
        cloud.renderer.transform.localScale = new Vector3(scale, scale, scale);
    }
}