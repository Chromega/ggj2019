using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthable : MonoBehaviour
{
    public int health = 10;

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
    }

    void Die()
    {
        Debug.Log("DEATH");
    }
}
