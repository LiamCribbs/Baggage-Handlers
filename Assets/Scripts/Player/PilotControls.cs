using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotControls : MonoBehaviour
{
    public Vector2 speed;
    public Vector2 acceleration;
    public Vector2 deceleration;
    public float outOfBoundsDeceleration;
    Vector2 moveAcceleration;

    public MoveBounds bounds;

    void Update()
    {
        Vector3 position = transform.localPosition;

        // Get wasd input
        Vector2 input = new Vector2();
        if (Input.GetKey(KeyCode.D))
        {
            input.x += 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input.x -= 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            input.y += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input.y -= 1f;
        }

        bool outOfBounds;

        // Lerp our speed value based on the move input
        if (input.x > 0f)
        {
            outOfBounds = position.x > bounds.Right;
            moveAcceleration.x = Mathf.Lerp(moveAcceleration.x, speed.x, acceleration.x * Time.deltaTime);
        }
        else if (input.x < 0f)
        {
            outOfBounds = position.x < bounds.Left;
            moveAcceleration.x = Mathf.Lerp(moveAcceleration.x, -speed.x, acceleration.x * Time.deltaTime);
        }
        else
        {
            outOfBounds = false;
            moveAcceleration.x = Mathf.Lerp(moveAcceleration.x, 0f, deceleration.x * Time.deltaTime);
        }

        // Slow down if we're out of bounds
        if (outOfBounds)
        {
            moveAcceleration.x = Mathf.Lerp(moveAcceleration.x, 0f, outOfBoundsDeceleration * Time.deltaTime);
        }

        if (input.y > 0f)
        {
            outOfBounds = position.y > bounds.Top;
            moveAcceleration.y = Mathf.Lerp(moveAcceleration.y, speed.y, acceleration.y * Time.deltaTime);
        }
        else if (input.y < 0f)
        {
            outOfBounds = position.y < bounds.Bottom;
            moveAcceleration.y = Mathf.Lerp(moveAcceleration.y, -speed.y, acceleration.y * Time.deltaTime);
        }
        else
        {
            outOfBounds = false;
            moveAcceleration.y = Mathf.Lerp(moveAcceleration.y, 0f, deceleration.y * Time.deltaTime);
        }

        if (outOfBounds)
        {
            moveAcceleration.y = Mathf.Lerp(moveAcceleration.y, 0f, outOfBoundsDeceleration * Time.deltaTime);
        }

        // Add our lerped speed value to our position
        position.x += moveAcceleration.x * Time.deltaTime;
        position.y += moveAcceleration.y * Time.deltaTime;

        // Clamp position to the outer bounds
        transform.localPosition = bounds.ClampToOuterBounds(position);
    }
}
