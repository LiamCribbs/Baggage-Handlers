using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LuggageLeftForce : MonoBehaviour
{

    Rigidbody2D rigidBody;
    public float minSpeed = 1.5f;
    public float maxSpeed = 4.5f;
    float speed;

    void Start()
    {
        
        //Fetch the Rigidbody from the GameObject with this script attached
        rigidBody = GetComponent<Rigidbody2D>();
    }

    

    void FixedUpdate()
    {
        
        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        rigidBody.AddForce(Vector2.left * Random.Range(minSpeed, maxSpeed));
    }

    

}
