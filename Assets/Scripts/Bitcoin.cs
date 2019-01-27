using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bitcoin : MonoBehaviour
{
    // Events
    public static System.Action addFundsEvent;

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
        // only the player layer will intersect this
        addFunds();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // only the player layer will intersect this
        addFunds();
    }

    void addFunds()
    {
        if (addFundsEvent != null) addFundsEvent();
        Destroy(gameObject);
    }
}
