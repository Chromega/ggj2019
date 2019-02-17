using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Movable
{
    public int queueOrder;
    public Castable weaponPrefab;

    Vector3 lastExactPathPosition;
    Vector3 exactPathPosition;
    Vector3 settlePosition;
    float settleAmount;
    bool isStill;
    private int direction = 1; // 1 = right, -1 = left
    public Transform weaponSpawnPoint;

    public AudioClip attackSoundEffect;
    public AudioClip swapSoundEffect;

    Castable currentSustainedCast = null;
    
    public enum State
    {
        Active,
        Following,
        Chillin
    }
    State state = State.Chillin;

    // Use this for initialization
    void Awake()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (!animator)
            animator = GetComponent<Animator>();

        Healthable healthable = GetComponent<Healthable>();
    }

    protected override bool getJump()
    {
        return Input.GetButtonDown("Jump");
    }

    protected override bool getJumpButtonHold()
    {
        return Input.GetButton("Jump");
    }

    protected override float getHorizontalDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(velocity.y) > Mathf.Epsilon) // if in the middle of a jump
        {
            horizontal = Input.GetAxis("Horizontal");
        }


        //lame jam code
        if (currentSustainedCast && currentSustainedCast is Shield)
        {
            horizontal = 0;
        }
        if (horizontal > 0)
        {
            direction = 1;
        }
        else if (horizontal < 0)
        {
            direction = -1;
        }
        return horizontal;
    }

    protected override void ComputeVelocity()
    {
        if (state == State.Active)
            base.ComputeVelocity();
        else
            targetVelocity = velocity;
    }

    void Start()
    {
        Healthable healthable = GetComponent<Healthable>();
    }

    protected override void Update()
    {
        if (state == State.Active)
        {
            base.Update();
        }

        UpdateAnimationProperties();

        if (state == State.Active && Input.GetButtonDown("Fire1") && weaponPrefab)
        {
            StartCoroutine(Cast());
        }
        if (Input.GetButtonUp("Fire1") && currentSustainedCast)
        {
            currentSustainedCast.EndCast();
            animator.SetBool("shield", false);
            currentSustainedCast = null;
        }

        if (state == State.Following)
        {
            Vector3 newPos;
            if (settleAmount > 0.01f)
            {
                Vector3 settleLerpPos = Vector3.MoveTowards(transform.position, settlePosition, 6f * Time.deltaTime);
                Vector3 pos = Vector3.Lerp(exactPathPosition, settleLerpPos, settleAmount);
                pos.y = exactPathPosition.y;
                newPos = pos;
            }
            else
            {
                newPos = exactPathPosition;
            }

            velocity = (newPos - transform.position) / Time.deltaTime;
            transform.position = newPos;

            if (isStill)
            {
                settleAmount = Mathf.MoveTowards(settleAmount, 1f, 2f * Time.deltaTime);
            }
            else
            {
                settleAmount = Mathf.MoveTowards(settleAmount, 0f, 2f * Time.deltaTime);

                // Set the sprite direction
                if (velocity.x > 0)
                {
                    direction = 1;
                }
                else if (velocity.x < 0)
                {
                    direction = -1;
                }
            }
        }

        if (animator.runtimeAnimatorController)
        {
            if (Mathf.Abs(velocity.x) > Mathf.Epsilon)
            {
                animator.SetBool("moving", true);
            }
            else
            {
                animator.SetBool("moving", false);
            }

            if (velocity.y > Mathf.Epsilon)
            {
                animator.SetBool("jumping", true);
                animator.SetBool("falling", false);
            }
            else if (velocity.y < -Mathf.Epsilon)
            {
                animator.SetBool("falling", true);
                animator.SetBool("jumping", false);
            }
            else
            {
                animator.SetBool("falling", false);
                animator.SetBool("jumping", false);
            }
        }
    }

    IEnumerator Cast()
    {
        // Attack animation and sound
        animator.SetTrigger("attack");
        AudioSource.PlayClipAtPoint(attackSoundEffect, new Vector3(0, 0, 0));
        if (weaponPrefab.GetCastTime() > 0)
            yield return new WaitForSeconds(weaponPrefab.GetCastTime());

        int weaponDirection = spriteRenderer.flipX ? -1 : 1;
        Vector3 weaponWorldPosition = weaponSpawnPoint.position;

        Vector3 weaponLocalPosition = transform.InverseTransformPoint(weaponWorldPosition);//new Vector3(transform.position.x + weaponDirection * 0.5f, transform.position.y + 0.5f, transform.position.z);
        if (spriteRenderer.flipX)
            weaponLocalPosition.x *= -1;
        weaponWorldPosition = transform.TransformPoint(weaponLocalPosition);
        Castable weapon = Instantiate(weaponPrefab, weaponWorldPosition, Quaternion.identity);
        //weapon.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

        if (weapon.GetCastType() == Castable.CastType.Sustained)
        {
            currentSustainedCast = weapon;
            animator.SetBool("shield", true);
        }

        if (weaponPrefab is Bullet)
        {
            Bullet bullet = (Bullet)weapon;
            bullet.direction = weaponDirection;
        }

        weapon.StartCast(this);
    }

    protected override void FixedUpdate()
    {
        if (state == State.Active)
            base.FixedUpdate();
        else
            grounded = true;
    }

    public int GetDirection()
    {
        return direction;
    }

    public void SetQueueOrder(int queueOrder)
    {
        this.queueOrder = queueOrder;
        if (spriteRenderer)
            spriteRenderer.sortingOrder = -1*queueOrder;
    }

    public void Activate()
    {
        collider2D.enabled = true;
        state = State.Active;

        AudioSource.PlayClipAtPoint(swapSoundEffect, new Vector3(0, 0, 0));
    }

    public void Deactivate()
    {
        collider2D.enabled = false;
        state = State.Following;

        if (currentSustainedCast)
        {
            currentSustainedCast.EndCast();
            currentSustainedCast = null;
        }
    }

    public void SetPosition(Vector3 pos, float parentFlipSign)
    {
        Vector3 posDiff = pos - lastExactPathPosition;
        //transform.position = pos;
        //velocity = posDiff / Time.deltaTime;

        settlePosition = pos - 1.2f * queueOrder * Vector3.right * parentFlipSign;
        lastExactPathPosition = exactPathPosition;
        exactPathPosition = pos;

        if ((posDiff).sqrMagnitude < .001f)
            isStill = true;
        else
            isStill = false;

        if (!Physics2D.Linecast(settlePosition, (Vector2)settlePosition + Vector2.down * .1f, (int)PhysicsUtl.LayerMasksBitmasks.Default))
            settlePosition = pos;
    }

    public void CopyFromOther(PlayerController controller)
    {
        transform.position = controller.transform.position;
        velocity = controller.velocity;
        targetVelocity = controller.targetVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state != State.Chillin)
            return;

        if (collision.collider.gameObject.layer == (int)PhysicsUtl.LayerMasks.Player)
        {
            PlayerChain.Instance.AddToChain(this);
        }
    }

    public void Die()
    {
        // called by Healthable, which this does NOT inherit from
        PlayerChain.Instance.RemoveFromChain(this);
    }
}