using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGun : MonoBehaviour
{
    public new Camera camera;

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
    int currentHarpoonIndex;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Get direction between this and mouse position, then convert it into a rotation (Atan2 converts direction to rad angle)
            Vector2 direction = ((Vector2)Input.mousePosition - (Vector2)camera.WorldToScreenPoint(transform.position)).normalized;
            Quaternion rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            // Spawn new harpoon
            Harpoon harpoon = Instantiate(harpoonPrefab, transform.position, rotation).GetComponent<Harpoon>();
            harpoon.Initialize(this);
            harpoon.Rigidbody.velocity = direction * shootSpeed;
            harpoons.Add(harpoon);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            // Reel in any harpoons
            //if (harpoons.Count > 0)
            //{
            //    // Reel in a different harpoon each frame
            //    Harpoon harpoon = harpoons[currentHarpoonIndex];
            //    harpoon.rope.SetLength(harpoon.rope.rope.maxLength - reelInSpeed * Time.deltaTime);

            //    if (harpoon.rope.rope.maxLength < lengthToCutRope)
            //    {
            //        // Set rope end to a new empty rigidbody
            //        harpoon.rope.attachedTransformRigidbody = SpawnEmptyRigidbody(transform.position);
            //        harpoon.rope.attachedTransform = harpoon.rope.attachedTransformRigidbody.transform;
            //        harpoon.rope.end = harpoon.rope.attachedTransform;

            //        harpoon.Detach();

            //        harpoon.rope.start = harpoon.transform;

            //        harpoons.Remove(harpoon);
            //        currentHarpoonIndex--;
            //    }

            //    currentHarpoonIndex = harpoons.Count == 0 ? 0 : (currentHarpoonIndex + 1) % harpoons.Count;
            //}

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

                    harpoon.Detach();

                    harpoon.rope.start = harpoon.transform;

                    harpoons.Remove(harpoon);
                    i--;
                }
            }
        }
    }

    public Rigidbody2D SpawnEmptyRigidbody(Vector3 position)
    {
        GameObject go = new GameObject("EmptyRigidbody");
        go.transform.position = position;

        Rigidbody2D rigidbody = go.AddComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(Random.Range(emptyRigidbodyVelXRange.x, emptyRigidbodyVelXRange.y),
            Random.Range(emptyRigidbodyVelYRange.x, emptyRigidbodyVelYRange.y));

        return rigidbody;
    }
}