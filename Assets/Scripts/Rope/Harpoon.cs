using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public PhysicsRope rope;

    public HarpoonGun Gun { get; private set; }
    public Rigidbody2D Rigidbody
    {
        get => rope.rigidbody;
        set => rope.rigidbody = value;
    }

    bool attached;

    public float attachForceMultiplier = 1f;

    public void Initialize(HarpoonGun gun)
    {
        Gun = gun;
        rope.end = gun.transform;
        rope.attachedTransform = rope.end;

        float value = Random.Range(0.2f, 1f);
        Color color = new Color(value, value, value);
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public void AttachToRigidbody(Rigidbody2D attachedRigidbody)
    {
        // Don't attach if we hit a Luggage that already has an attached harpoon
        if (attachedRigidbody.TryGetComponent(out Luggage hitLuggage) && hitLuggage.AttachedHarpoon)
        {
            return;
        }

        Vector2 velocity = Rigidbody.velocity;

        attached = true;
        // Destroy current rigidbody if it's on this gameObject
        if (Rigidbody.gameObject == gameObject)
        {
            Destroy(Rigidbody);
        }

        // Set new rigidbody/parent
        Rigidbody = attachedRigidbody;
        transform.SetParent(Rigidbody.transform);

        // Add velocity to rigidbody
        Rigidbody.AddForce(velocity * attachForceMultiplier, ForceMode2D.Impulse);

        // Assign ourselves to the hit luggage object
        if (hitLuggage)
        {
            hitLuggage.AttachedHarpoon = this;
        }
    }

    public void Detach()
    {
        // Don't do anything if we're already detached
        if (!attached)
        {
            return;
        }

        attached = false;

        if (Rigidbody.TryGetComponent(out Luggage hitLuggage))
        {
            hitLuggage.AttachedHarpoon = null;
        }

        Vector3 velocity = Rigidbody.velocity;
        Rigidbody = gameObject.AddComponent<Rigidbody2D>();
        Rigidbody.velocity = velocity;
        transform.SetParent(null);

        Rigidbody.mass = 5f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attached && collision.attachedRigidbody && !collision.GetComponent<Harpoon>())
        {
            AttachToRigidbody(collision.attachedRigidbody);
        }
    }
}