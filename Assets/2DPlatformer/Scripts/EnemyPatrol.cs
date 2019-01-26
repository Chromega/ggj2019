using UnityEngine;

public class EnemyPatrol : Movable
{
    public float visionRange = 5; 
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
        // Patrol in a random direction
        float probability = Random.Range(0.0f, 1.0f);
        if (probability > 0.97f)
        {
            direction *= -1;
        }

        // If you see the player, walk toward the player
        Vector2 playerPosition = PlayerChain.Instance.transform.position;
        Vector2 position = this.transform.position;

        if (Mathf.Abs(playerPosition.x - position.x) < visionRange)
        {
            Debug.Log("I SEE YOU");
            direction = (playerPosition.x - position.x) / (Mathf.Abs(playerPosition.x - position.x));
        }



        return direction;
    }
}
