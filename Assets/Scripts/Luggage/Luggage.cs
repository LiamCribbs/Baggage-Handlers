using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luggage : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    public Collider2D[] colliders;
    
    public Harpoon AttachedHarpoon { get; set; }

    public float windMultiplier = 1f;

    const float ShrinkSpeed = 2f;

    float initialGravityScale;

    void Start()
    {
        DestroyTrigger.instance.destroyableObjects.Add(transform);
        initialGravityScale = rigidbody.gravityScale;
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(Weather.Wind * (windMultiplier * Time.fixedDeltaTime));
    }

    public void AttachHarpoon(Harpoon harpoon)
    {
        AttachedHarpoon = harpoon;
        rigidbody.gravityScale = 1f;
    }

    public void DetachHarpoon()
    {
        AttachedHarpoon = null;
        rigidbody.gravityScale = initialGravityScale;
    }

    public IEnumerator ShrinkAndDestroy()
    {
        float initialScale = transform.localScale.x;
        Vector2 initialVelocity = rigidbody.velocity;
        float time = 0f;

        while (time < 1f)
        {
            time += ShrinkSpeed * Time.deltaTime;
            if (time > 1f)
            {
                time = 1f;
            }

            float scale = initialScale * (1f - Pigeon.EaseFunctions.EaseInQuartic(time));
            transform.localScale = new Vector3(scale, scale, scale);

            rigidbody.velocity = initialVelocity * (1f - Pigeon.EaseFunctions.EaseOutCubic(time));

            yield return null;
        }

        DestroyTrigger.instance.destroyableObjects.Remove(transform);
        Destroy(gameObject);
    }
}