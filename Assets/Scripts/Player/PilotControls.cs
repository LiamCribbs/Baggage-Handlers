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

    public Vector4 moveRotation;
    public float rotationSpeed;

    public MoveBounds bounds;

    public int maxHealth;
    public int Health { get; private set; }

    public int planeCollisionDamage;
    public int debrisCollisionDamage;

    public float takeDamageCooldown;
    Coroutine damageCooldownCoroutine;

    public float hitDebrisShake;
    public float hitPlaneShake;

    void Start()
    {
        Health = maxHealth;
    }

    void Update()
    {
        Vector3 position = transform.localPosition;

        // Get wasd input
        Vector2 input = new Vector2();
        float angle = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            input.x += 1f;
            angle += moveRotation.x;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input.x -= 1f;
            angle += moveRotation.y;
        }
        if (Input.GetKey(KeyCode.W))
        {
            input.y += 1f;
            angle += moveRotation.z;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input.y -= 1f;
            angle += moveRotation.w;
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

        transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(transform.localEulerAngles.z, angle, rotationSpeed * Time.deltaTime));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = collision.collider.gameObject.layer;
        if (layer == DeployDebris.DebrisLayer)
        {
            collision.collider.enabled = false;
            OnHitDebris();
        }
        else if (layer == LuggageManager.LuggagePlaneLayer)
        {
            OnHitPlane();
        }
    }

    void OnHitDebris()
    {
        if (damageCooldownCoroutine != null)
        {
            return;
        }

        TakeDamage(debrisCollisionDamage);
        CameraController.instance.Shake(hitDebrisShake);
    }

    void OnHitPlane()
    {
        if (damageCooldownCoroutine != null)
        {
            return;
        }

        TakeDamage(planeCollisionDamage);
        CameraController.instance.Shake(hitPlaneShake);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
        else
        {
            damageCooldownCoroutine = StartCoroutine(DamageCooldownCoroutine());
        }

        ScoreManager.instance.UpdateHealthBar(this);
    }

    public void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2/*UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex*/);
    }

    IEnumerator DamageCooldownCoroutine()
    {
        float time = 0f;
        while (time < takeDamageCooldown)
        {
            time += Time.deltaTime;
            yield return null;
        }

        damageCooldownCoroutine = null;
    }
}