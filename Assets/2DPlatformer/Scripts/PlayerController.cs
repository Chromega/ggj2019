using UnityEngine;

public class PlayerController : Movable
{
    public int queueOrder;
    public Collider2D collider2D;
    public Weaponable weaponPrefab;

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

        if (Input.GetButtonUp("Fire1") && weaponPrefab)
        {
            int weaponDirection = spriteRenderer.flipX ? -1 : 1;
            Vector3 weaponPosition = new Vector3(transform.position.x + weaponDirection * 0.5f, transform.position.y + 0.5f, transform.position.z);
            Weaponable weapon = Instantiate(weaponPrefab, weaponPosition, Quaternion.identity);
            weapon.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

            if (weaponPrefab is Bullet)
            {
                Bullet bullet = (Bullet)weapon;
                bullet.velocity = new Vector3(weaponDirection, 0, 0);
            }
        }
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
        if (spriteRenderer)
            spriteRenderer.sortingOrder = -1*queueOrder;
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

    public void CopyFromOther(PlayerController controller)
    {
        transform.position = controller.transform.position;
        velocity = controller.velocity;
        targetVelocity = controller.targetVelocity;
    }
}