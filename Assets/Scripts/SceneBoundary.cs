using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBoundary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Healthable healthable = col.gameObject.GetComponent<Healthable>();
        if (healthable)
        {
            healthable.TakeDamage(9999, gameObject);
        }
        else if (col.gameObject.GetComponent<Bullet>())
        {
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Healthable healthable = col.gameObject.GetComponent<Healthable>();
        if (healthable)
        {
            healthable.TakeDamage(9999, gameObject);
        }
        else if (col.gameObject.GetComponent<Bullet>())
        {
            Destroy(col.gameObject);
        }
    }

    void destroyObjects()
    {

    }
}
