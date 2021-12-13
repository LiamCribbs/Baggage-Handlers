using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGun : MonoBehaviour
{
    public new Camera camera;

    [Space(20)]
    public int maxHarpoons;

    [Space(20)]
    public float shootSpeed;
    public float reelInSpeed;
    public float lengthToCutRope;

    [Space(20)]
    public Vector2 emptyRigidbodyVelXRange;
    public Vector2 emptyRigidbodyVelYRange;

    [Space(20)]
    public GameObject harpoonPrefab;

    readonly List<Harpoon> harpoons = new List<Harpoon>(16);

    public AudioSource audioSource;
    public AudioClip shootClip;
    public float shootVolume;

    public AudioSource reelSource;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (harpoons.Count >= maxHarpoons)
            {
                return;
            }

            // Get direction between this and mouse position, then convert it into a rotation (Atan2 converts direction to rad angle)
            Vector2 direction = ((Vector2)Input.mousePosition - (Vector2)camera.WorldToScreenPoint(transform.position)).normalized;
            Quaternion rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            // Spawn new harpoon
            Harpoon harpoon = Instantiate(harpoonPrefab, transform.position, rotation).GetComponent<Harpoon>();
            harpoon.Initialize(this);
            harpoon.Rigidbody.velocity = direction * shootSpeed;
            harpoons.Add(harpoon);

            ScoreManager.instance.harpoonsText.text = harpoons.Count.ToString();

            audioSource.PlayOneShot(shootClip, shootVolume);
        }

        // Reel in
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (harpoons.Count > 0)
            {
                if (!reelSource.isPlaying)
                {
                    reelSource.Play();
                }
            }
            else if (reelSource.isPlaying)
            {
                reelSource.Stop();
            }

            for (int i = 0; i < harpoons.Count; i++)
            {
                Harpoon harpoon = harpoons[i];
                harpoon.rope.SetLength(harpoon.rope.rope.maxLength - reelInSpeed * Time.deltaTime);

                if (harpoon.rope.rope.maxLength < lengthToCutRope)
                {
                    // Set rope end to a new empty rigidbody
                    harpoon.rope.attachedTransformRigidbody = SpawnEmptyRigidbody(transform.position);
                    harpoon.rope.attachedTransform = harpoon.rope.attachedTransformRigidbody.transform;
                    harpoon.rope.end = harpoon.rope.attachedTransform;

                    Luggage attachedLuggage = harpoon.Detach();
                    if (attachedLuggage != null)
                    {
                        CollectLuggage(attachedLuggage);
                    }

                    harpoon.collider.enabled = false;

                    harpoon.rope.start = harpoon.transform;

                    harpoons.Remove(harpoon);
                    ScoreManager.instance.harpoonsText.text = harpoons.Count.ToString();
                    i--;
                }
            }
        }
        else if (reelSource.isPlaying)
        {
            reelSource.Stop();
        }
    }

    void CollectLuggage(Luggage luggage)
    {
        for (int i = 0; i < luggage.colliders.Length; i++)
        {
            luggage.colliders[i].enabled = false;
        }

        ScoreManager.AddScore(1);

        luggage.StartCoroutine(luggage.ShrinkAndDestroy());
        //luggage.rigidbody.gravityScale *= 0.5f;
        //luggage.rigidbody.drag *= 2f;
    }

    public Rigidbody2D SpawnEmptyRigidbody(Vector3 position)
    {
        GameObject go = new GameObject("EmptyRigidbody");
        go.transform.position = position;
        DestroyTrigger.instance.destroyableObjects.Add(go.transform);

        Rigidbody2D rigidbody = go.AddComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(Random.Range(emptyRigidbodyVelXRange.x, emptyRigidbodyVelXRange.y),
            Random.Range(emptyRigidbodyVelYRange.x, emptyRigidbodyVelYRange.y));

        return rigidbody;
    }
}