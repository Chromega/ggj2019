using UnityEngine;

public class EnemyPatrol : Movable
{

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override bool getJump()
    {
        return false;
    }

    protected override float getHorizontalDirection()
    {
        float randomDirection = Random.Range(0.0f, 1.0f);
        Debug.Log(randomDirection);
        return randomDirection;
    }
}
