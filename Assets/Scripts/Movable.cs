using UnityEngine;
using System.Collections;

public abstract class Movable : PhysicsObject
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    new public Collider2D collider2D;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 22;
    public float horizontalSpeed = 1.25f;
    public float recoilTakeoffSpeed = 0.8f;
    public float fasterFallMultiplier = 3.0f;
    public float shortJumpMultiplier = 15.0f;

    private bool isRecoil = false;
    private int recoilDirection;

    public bool isBackwards;

    // These are set by either the AI or a key press
    protected abstract float getHorizontalDirection();
    protected abstract bool getJump();
    protected abstract bool getJumpButtonHold();

    protected override void ComputeVelocity()
    {
        // Apply greater gravity on the fall for snappier jumps
        // See https://www.youtube.com/watch?v=7KiK0Aqtmzc

        Vector2 move = Vector2.zero;

        if (isRecoil)
        {
            int direction = recoilDirection;
            velocity.y = recoilTakeoffSpeed;
            move.x = (float)(direction);
        }
        else
        {
            // Handle horizontal
            move.x = this.getHorizontalDirection() * horizontalSpeed;

            // Handle jump
            if (getJump() && grounded)
            {
                // initial launch speed
                velocity.y = jumpTakeOffSpeed;
            }
            else if (velocity.y < 0)
            {
                // Faster fall for better feeling physics
                velocity += Vector2.up * Physics2D.gravity.y * fasterFallMultiplier * Time.deltaTime;
            }
            else if (velocity.y > 0 && !getJumpButtonHold())
            {
                // Enable a short jump by accelerating the down
                velocity += Vector2.up * Physics2D.gravity.y * shortJumpMultiplier * Time.deltaTime;
            }
        }
        targetVelocity = move * maxSpeed;
    }

    public void Recoil(Vector3 hitDirection)
    {
        isRecoil = true;
        recoilDirection = Mathf.RoundToInt((hitDirection.x / Mathf.Abs(hitDirection.x))); 
        StartCoroutine("FinishRecoilTimer");
    }

    IEnumerator FinishRecoilTimer()
    {
        yield return new WaitForSeconds(.1f);
        isRecoil = false; 
    }


    protected virtual void UpdateAnimationProperties()
    {
        // Set the correct direction of the sprite
        if (!isRecoil)
        {
            if (velocity.x < -0.01f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (velocity.x > 0.01f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            if (velocity.x < -0.01f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (velocity.x > 0.01f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (animator.runtimeAnimatorController)
        {
            animator.logWarnings = false;
            animator.SetBool("grounded", grounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            animator.SetBool("moving", Mathf.Abs(velocity.x) > .01f);
        }
    }
}
