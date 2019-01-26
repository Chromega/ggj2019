using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 velocity;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Healthable healthable = col.gameObject.GetComponent<Healthable>();
        if (healthable)
        {
            healthable.TakeDamage(1);
        }
        Destroy(gameObject);
    }
}
