using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGun : MonoBehaviour
{
    public new Camera camera;

    public float shootSpeed;

    public GameObject harpoonPrefab;

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
        }
    }
}