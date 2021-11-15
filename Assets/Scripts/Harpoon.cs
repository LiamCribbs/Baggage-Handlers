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
        // Destroy current rigidbody if it's on this gameObject
        if (Rigidbody.gameObject == gameObject)
        {
            Destroy(Rigidbody);
        }

        Rigidbody = attachedRigidbody;
        transform.SetParent(Rigidbody.transform);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody && !collision.GetComponent<Harpoon>())
        {
            AttachToRigidbody(collision.attachedRigidbody);
        }
    }
}