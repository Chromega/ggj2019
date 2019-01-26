using UnityEngine;

public class EnemyPatrol : Movable
{

    private float direction = -1.0f;

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
        float probability = Random.Range(0.0f, 1.0f);
        if (probability > 0.97f)
        {
            direction *= -1;
        }

        return direction;
    }
}
