using UnityEngine;

public class PlayerController : Movable
{
    public Weaponable weapon;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override bool getJump()
    {
        return Input.GetButtonDown("Jump");
    }

    protected override float getHorizontalDirection()
    {
        return Input.GetAxis("Horizontal");
    }
}