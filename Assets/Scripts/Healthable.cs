using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthable : MonoBehaviour
{
    public int health = 10;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Movable movable; 

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        movable = GetComponent<Movable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage, GameObject g)
    {
        health -= damage;

        // Flash red on the sprite for damage taken
        StartCoroutine("FlashDamageAnimation");

        if (movable) movable.Recoil((transform.position - g.transform.position).normalized);

        // Adding a damage marker
        GameObject damageMarkerPrefab = Resources.Load<GameObject>("DamageCounter");
        GameObject damageMarker = Instantiate(damageMarkerPrefab); 

        //damageMarker.transform.SetParent(this.transform);
        damageMarker.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, -2);
        damageMarker.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        TextMesh text = damageMarker.GetComponent<TextMesh>();
        text.text = "-" + damage.ToString();

        StartCoroutine(FadeDamageMarker(damageMarker));

        PlayerController player = GetComponent<PlayerController>();
        if (player) player.updateFundsLeft(health);
    }

    IEnumerator FadeDamageMarker(GameObject damageMarker)
    {
        // Float up and fade out
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            damageMarker.transform.position = new Vector3(damageMarker.transform.position.x, damageMarker.transform.position.y + 0.1f, -2);

            TextMesh text = damageMarker.GetComponent<TextMesh>();
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.05f);

            yield return new WaitForSeconds(.05f);
        }

        Destroy(damageMarker);
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
            }
            else
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                red = false; 
            }
            yield return new WaitForSeconds(.05f);
        }
    }

    public virtual void Die()
    {
    }
}
