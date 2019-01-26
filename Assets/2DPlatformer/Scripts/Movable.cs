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

            if (getJump() && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.2f;
                }
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

        // FIXME(amber): Set player direction back to the original at the end of the recoil. 
        // FIXED(seanyliu): velocity no longer sets the player's direction
    }


    protected virtual void UpdateAnimationProperties()
    {
        bool flipSprite = (spriteRenderer.flipX ? (velocity.x > 0.01f) : (velocity.x < -0.01f));
        if (gameObject.GetComponent<PlayerController>()) {
            // for PlayerController, set the direction based on GetDirection() which is set by keyboard
            spriteRenderer.flipX = (gameObject.GetComponent<PlayerController>().GetDirection() < 0);
        } else if (flipSprite && !isRecoil)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
    }
}
