using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weaponable : MonoBehaviour
{
    public int damageToDeal = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract void finishCollision();

    void OnTriggerEnter2D(Collider2D col)
    {
        dealDamage(col.gameObject.GetComponent<Healthable>());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        dealDamage(col.gameObject.GetComponent<Healthable>());
    }

    void dealDamage(Healthable healthable)
    {
        if (healthable)
        {
            healthable.TakeDamage(damageToDeal, gameObject);
            finishCollision();
        }
    }
}
