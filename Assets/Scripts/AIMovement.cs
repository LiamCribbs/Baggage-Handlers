using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pigeon;
using Pigeon.Math;

public class AIMovement : MonoBehaviour
{
    public float speed;
    public Vector2 waitTime;

    public MoveBounds[] moveBounds;
    public MoveBounds currentBounds;

    void Start()
    {
        StartCoroutine(Move());
    }

    //void Update()
    //{
    //    transform.position = vector2.movetowards(transform.position, movespots[randomspot].position, speed * time.deltatime);

    //    if (vector2.distance(transform.position, movespots[randomspot].position) < 0.2f)
    //    {
    //        if (waittime <= 0)
    //        {
    //            randomspot = random.range(0, movespots.length);
    //            waittime = startwaittime;
    //        }
    //        else
    //        {
    //            waittime -= time.deltatime;
    //        }
    //    }
    //}

    IEnumerator Move()
    {
        while (true)
        {
            Vector2 startPos = transform.localPosition;
            Vector2 endPos = currentBounds.RandomPointWorld();
            float distance = (endPos - startPos).MagFast();
            float speed = 1f / (distance / this.speed);

            float time = 0f;

            while (time < 1f)
            {
                time += speed * Time.deltaTime;
                if (time > 1f)
                {
                    time = 1f;
                }

                transform.localPosition = Vector2.Lerp(startPos, endPos, EaseFunctions.EaseInOutCubic(time));
                yield return null;
            }

            float waitTime = UnityEngine.Random.Range(this.waitTime.x, this.waitTime.y);
            time = 0f;

            while (time < waitTime)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
 