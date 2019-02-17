using UnityEngine;
using System.Collections;

public abstract class Movable : PhysicsObject
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    new public Collider2D collider2D;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 3;
    public float horizontalSpeed = 0.8f;
    public float recoilTakeoffSpeed = 0.8f;

    private bool isRecoil = false;
    private int recoilDirection;

    public bool isBackwards;

    protected abstract float getHorizontalDirection();
    protected abstract bool getJump(); 

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (isRecoil)
        {
            int direction = recoilDirection;

            velocity.y = recoilTakeoffSpeed;
            move.x = (float)(direction);
        } else {
            move.x = this.getHorizontalDirection() * horizontalSpeed;

            // Handle the jump
            if (getJump() && grounded)
            {
                // initial launch speed
                //Debug.Log("JUMPING");
                velocity.y = jumpTakeOffSpeed;
                //Debug.Log("setting velocity to" + velocity.ToString());
            }
            else if (getJump() && velocity.y > 0)
            {
                //Debug.Log("mid jump");
                // This handles enabling short vs long jumps
                velocity.y = velocity.y * Time.deltaTime;
            }
            else if (velocity.y < 0)
            {
                // Apply greater gravity on the fall for snappier jumps
                // https://www.youtube.com/watch?v=7KiK0Aqtmzc
                velocity += Vector2.up * Physics2D.gravity.y * 1.8f * Time.deltaTime;
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
