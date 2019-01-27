using UnityEngine;

public class EnemyPatrol : Movable
{
    public float visionRange = 5; 
    private float direction = -1.0f;


    void Awake()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (!animator)
            animator = GetComponent<Animator>();
    }

    protected override bool getJump()
    {
        return false;
    }

    protected override void Update()
    {
        base.Update();
        UpdateAnimationProperties();
    }

    protected void flipRandomDirection()
    {
        float probability = Random.Range(0.0f, 1.0f);
        if (probability > 0.98f)
        {
            direction *= -1;
        }
    }

    protected override float getHorizontalDirection()
    {
        // Patrol in a random direction
        flipRandomDirection(); 

        // Stop if you're on a ledge
        bool leftGround;
        bool rightGround;
        PhysicsUtl.LedgeCheck(collider2D, out leftGround, out rightGround);

        float directionToReturn = direction;
        if (direction < 0 && !leftGround)
            directionToReturn = 0;
        else if (direction > 0 && !rightGround)
            directionToReturn = 0;

        return directionToReturn;
    }
}
