using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthableHipo : Healthable
{
    // Events
    public static System.Action onDied;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if died
        if (health <= 0)
        {
            if (onDied != null)
            {
                onDied();
            }
            Die();
        }
    }
}
