using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthable : MonoBehaviour
{
    public int health = 10;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Flash red on the sprite for damage taken
        StartCoroutine("FlashDamageAnimation");
    }

    IEnumerator FlashDamageAnimation()
    {
        bool red = false;
        for (float f = 0.3f; f >= 0; f -= 0.1f)
        {
            if (!red)
            {
                spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
                red = true;
                Debug.Log("switching to red");
            }
            else
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                red = false; 
                Debug.Log("switching to white");
            }
            yield return new WaitForSeconds(.05f);
        }
    }

    void Die()
    {
        Debug.Log("DEATH");
        Destroy(gameObject); 
    }
}
