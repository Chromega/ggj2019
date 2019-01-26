﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weaponable
{
    public Vector3 velocity;
    public int direction = 1;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = velocity * direction;
    }
}
