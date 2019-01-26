using UnityEngine;

public class PlayerController : Movable
{
    public int queueOrder;
    public Collider2D collider2D;
    public Weaponable weapon;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override bool getJump()
    {
        return Input.GetButtonDown("Jump");
    }

    protected override float getHorizontalDirection()
    {
        return Input.GetAxis("Horizontal");
    }

    protected override void ComputeVelocity()
    {
        if (queueOrder == 0)
            base.ComputeVelocity();
        else
            targetVelocity = velocity;
    }

    protected override void Update()
    {
        if (queueOrder == 0)
            base.Update();
        UpdateAnimationProperties();
    }

    protected override void FixedUpdate()
    {
        if (queueOrder == 0)
            base.FixedUpdate();
        else
            grounded = true;
    }

    public void SetQueueOrder(int queueOrder)
    {
        this.queueOrder = queueOrder;
    }

    public void Activate()
    {
        collider2D.enabled = true;
    }

    public void Deactivate()
    {
        collider2D.enabled = false;
    }

    public void SetPosition(Vector3 pos)
    {
        Vector3 posDiff = pos - transform.position;
        transform.position = pos;
        velocity = posDiff / Time.deltaTime;
    }
}