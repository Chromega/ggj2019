using UnityEngine;
using System.Collections;

public abstract class Movable : PhysicsObject
{

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 3;
    public float horizontalSpeed = 0.8f;

    protected abstract float getHorizontalDirection();
    protected abstract bool getJump(); 

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

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

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}
