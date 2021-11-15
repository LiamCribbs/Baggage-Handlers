using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotControls : MonoBehaviour
{
    public new Rigidbody2D rigidbody;

    public float moveSpeed;


    Vector2 input;

    void Update()
    {
        input = new Vector2(Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f, Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(-15, -9, -5);
        }
    }

    void FixedUpdate()
    {
        rigidbody.velocity = input * (moveSpeed * Time.deltaTime);
    }
}
