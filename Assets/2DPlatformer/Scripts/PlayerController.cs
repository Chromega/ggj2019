using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Movable
{
    public int queueOrder;
    public Weaponable weaponPrefab;
    public Text fundsLeftText;

    Vector3 lastExactPathPosition;
    Vector3 exactPathPosition;
    Vector3 settlePosition;
    float settleAmount;
    bool isStill;
    private int direction = 1; // 1 = right, -1 = left
    
    public enum State
    {
        Active,
        Following,
        Chillin
    }
    State state = State.Chillin;
    public static System.Action onDied;

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

    protected override float getHorizontalDirection()
    {
        float horizontal = Input.GetAxis("Horizontal");
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
        updateFundsLeft(healthable.health);
    }

    protected override void Update()
    {
        if (state == State.Active)
        {
            base.Update();
        }

        UpdateAnimationProperties();
        
        // check if died
        Healthable healthable = GetComponent<Healthable>();
        if (healthable.health <= 0)
        {
            if (onDied != null)
            {
                onDied();
            }
        }
        
        if (state == State.Active && Input.GetButtonUp("Fire1") && weaponPrefab)
        {
            int weaponDirection = spriteRenderer.flipX ? -1 : 1;
            Vector3 weaponPosition = new Vector3(transform.position.x + weaponDirection * 0.5f, transform.position.y + 0.5f, transform.position.z);
            Weaponable weapon = Instantiate(weaponPrefab, weaponPosition, Quaternion.identity);
            weapon.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

            if (weaponPrefab is Bullet)
            {
                Bullet bullet = (Bullet)weapon;
                bullet.direction = weaponDirection;
            }
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
            else {
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
    }

    public void Deactivate()
    {
        collider2D.enabled = false;
        state = State.Following;
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

    public void updateFundsLeft(int hp)
    {
        fundsLeftText.text = "Funds Left: $" + hp.ToString();
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
}