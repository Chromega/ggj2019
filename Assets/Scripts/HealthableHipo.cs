using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthableHipo : Healthable
{

    public GameObject spawnPrefab;
    public float spawnTime = 3f; // How long between each spawn.

    // Events
    public static System.Action onDied;
    
    // Called before the first frame update
    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
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

    void Spawn ()
    {
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(spawnPrefab, transform.position, transform.rotation);
    }
}
