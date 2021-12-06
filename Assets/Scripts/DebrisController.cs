using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisController : MonoBehaviour
{
    public float speed = 10.0f;
    private Rigidbody2D rb;
    private Vector2 screenBounds;

    //public Transform leftObject;
    //public Transform rightObject;

    //public GameObject debrisPrefab;
    //public float respawnTime = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
       
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed, 0);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < (screenBounds.x * -1) - 3)
        {
            Destroy(this.gameObject); 
        }
    }
}
