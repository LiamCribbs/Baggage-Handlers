using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    public static DestroyTrigger instance;

    public List<Transform> destroyableObjects = new List<Transform>(64);

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Vector3 destroyPos = transform.localPosition;

        for (int i = 0; i < destroyableObjects.Count; i++)
        {
            Vector3 pos = destroyableObjects[i].position;
            if (pos.x < destroyPos.x || pos.y < destroyPos.y)
            {
                Destroy(destroyableObjects[i].gameObject);
                destroyableObjects.RemoveAt(i);
                i--;
            }
        }
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Destroy(collision.gameObject);
    //}
}