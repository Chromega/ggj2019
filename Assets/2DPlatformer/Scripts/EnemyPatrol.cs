using System.Collections;
using UnityEngine;

public class EnemyPatrol : Movable
{
    public float visionRange = 5; 
    private float direction = -1.0f;
    protected bool patrolling = true;


    void Awake()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (!animator)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(MoveLoop());
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

    IEnumerator MoveLoop()
    {
        while (true)
        {
            direction = 0;
            yield return new WaitForSeconds(Random.Range(.2f, .5f));
            direction = Random.Range(0f,1f) > .5f ? -1 : 1;
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        }
    }

    protected override float getHorizontalDirection()
    {
        // Patrol in a random direction
        //flipRandomDirection(); 

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
