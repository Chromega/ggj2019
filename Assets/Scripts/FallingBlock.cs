using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public float timeToFall = 1.0f;
    private bool fallActivated = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fallActivated)
        {
            timeToFall -= Time.deltaTime;
            if (timeToFall <= 0)
            {
                this.gameObject.AddComponent<Rigidbody2D>();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
        if (playerController)
        {
            fallActivated = true;
        }
    }
}
