using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public PhysicsRope rope;
    public new Collider2D collider;

    public HarpoonGun Gun { get; private set; }
    public Rigidbody2D Rigidbody
    {
        get => rope.rigidbody;
        set => rope.rigidbody = value;
    }

    bool attached;

    public float attachForceMultiplier = 1f;

    void Start()
    {
        DestroyTrigger.instance.destroyableObjects.Add(transform);
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
        // Don't attach if we hit a Luggage that already has an attached harpoon
        if (attachedRigidbody.TryGetComponent(out Luggage hitLuggage) && hitLuggage.AttachedHarpoon)
        {
            return;
        }

        if (attachedRigidbody.gameObject.layer == DeployDebris.DebrisLayer)
        {
            attachedRigidbody.GetComponent<Collider2D>().enabled = false;
        }

        Vector2 velocity = Rigidbody.velocity;

        // Pop out luggage contents
        if (hitLuggage != null && hitLuggage.popLuggage && Random.value <= LuggageManager.instance.luggagePopChance)
        {
            int luggagePops = Random.Range(1, LuggageManager.instance.luggagePopCountMax + 1);
            for (int i = 0; i < luggagePops; i++)
            {
                Rigidbody2D luggageItem = Instantiate(LuggageManager.instance.luggagePopPrefab, attachedRigidbody.transform.localPosition, Quaternion.Euler(0f, 0f, Random.value * 360f)).GetComponent<Rigidbody2D>();
                SpriteRenderer luggageItemSprite = luggageItem.GetComponent<SpriteRenderer>();
                luggageItemSprite.sprite = LuggageManager.instance.luggagePopSprites[Random.Range(0, LuggageManager.instance.luggagePopSprites.Length)];
                luggageItemSprite.color = Random.ColorHSV(0f, 1f, 0f, 1f, 0.7f, 1f);
                luggageItem.velocity = attachedRigidbody.velocity + new Vector2(Random.Range(-LuggageManager.instance.luggagePopVelocity.x, LuggageManager.instance.luggagePopVelocity.x),
                    Random.Range(-LuggageManager.instance.luggagePopVelocity.y, LuggageManager.instance.luggagePopVelocity.y));
                DestroyTrigger.instance.destroyableObjects.Add(luggageItem.transform);
            }
        }

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
            hitLuggage.AttachHarpoon(this);
            AudioClips.instance.source.PlayOneShot(AudioClips.instance.luggageHit[Random.Range(0, AudioClips.instance.luggageHit.Length)]);
        }
    }

    public Luggage Detach()
    {
        // Don't do anything if we're already detached
        if (!attached)
        {
            return null;
        }

        attached = false;

        if (Rigidbody.TryGetComponent(out Luggage hitLuggage))
        {
            hitLuggage.DetachHarpoon();
        }

        Vector3 velocity = Rigidbody.velocity;
        Rigidbody = gameObject.AddComponent<Rigidbody2D>();
        Rigidbody.velocity = velocity;
        transform.SetParent(null);

        Rigidbody.mass = 5f;

        return hitLuggage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attached && collision.attachedRigidbody && !collision.GetComponent<Harpoon>())
        {
            AttachToRigidbody(collision.attachedRigidbody);
        }
    }
}