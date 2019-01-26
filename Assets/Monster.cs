using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Weaponable
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Movable movable; 

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        movable = GetComponent<Movable>();
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void finishCollision()
    {
        // Bounce back a bit
        //movable.Recoil();
    }


}
