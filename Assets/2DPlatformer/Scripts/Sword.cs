using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weaponable
{
    private float timeToDisplay = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void finishCollision()
    { }


    // Update is called once per frame
    void FixedUpdate()
    {
        timeToDisplay -= Time.deltaTime;

        if (timeToDisplay < 0)
        {
            Destroy(gameObject);
        }
    }
}
