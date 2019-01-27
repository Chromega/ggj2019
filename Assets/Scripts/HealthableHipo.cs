using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthableHipo : Healthable
{

    public GameObject[] spawnPrefabs;
    public float spawnTime = 3f; // How long between each spawn.
    public Animator animator;
    public Transform spawnPos;
    bool isBackwards = true;
    public AudioClip spawnSoundEffect;

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

    IEnumerator Spawn ()
    {
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(.65f);

        Vector3 weaponWorldPosition = spawnPos.position;

        Vector3 weaponLocalPosition = transform.InverseTransformPoint(weaponWorldPosition);//new Vector3(transform.position.x + weaponDirection * 0.5f, transform.position.y + 0.5f, transform.position.z);
        if (spriteRenderer.flipX) weaponLocalPosition.x *= -1;
        //if (isBackwards) weaponLocalPosition.x *= -1;
        weaponWorldPosition = transform.TransformPoint(weaponLocalPosition);

        GameObject spawnPrefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        spawnedThings.Add(Instantiate(spawnPrefab, weaponWorldPosition, transform.rotation));
        if (spawnSoundEffect) AudioSource.PlayClipAtPoint(spawnSoundEffect, new Vector3(0, 0, 0), 1f);
    }
    

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if (spriteRenderer.isVisible)
            {
                spawnedThings.RemoveWhere((thing) => { return thing == null; });
                if (spawnedThings.Count < 10)
                    StartCoroutine(Spawn());
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
