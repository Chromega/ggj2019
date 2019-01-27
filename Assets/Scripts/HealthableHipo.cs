using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthableHipo : Healthable
{

    public GameObject spawnPrefab;
    public float spawnTime = 3f; // How long between each spawn.

    // Events
    public static System.Action onDied;

    HashSet<GameObject> spawnedThings = new HashSet<GameObject>();
    
    // Called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
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
        spawnedThings.Add(Instantiate(spawnPrefab, transform.position, transform.rotation));
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if (spriteRenderer.isVisible)
            {
                spawnedThings.RemoveWhere((thing) => { return thing == null; });
                if (spawnedThings.Count < 10)
                    Spawn();
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
