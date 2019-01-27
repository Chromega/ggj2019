using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyPatrol
{
    public float shootingRange = 3;
    public Transform weaponSpawnPoint;
    public AudioClip attackSoundEffect;
    public Castable weaponPrefab;

    private bool inRange = false;
    private bool currentlyShooting = false;

    protected override void Update()
    {
        base.Update();

        Vector2 playerPosition = PlayerChain.Instance.transform.position;
        Vector2 position = this.transform.position;

        float distanceXToPlayer = Mathf.Abs(playerPosition.x - position.x);
        float distanceYToPlayer = Mathf.Abs(playerPosition.y - position.y);
        if (distanceXToPlayer < shootingRange && distanceYToPlayer < shootingRange)
        {
            if (!currentlyShooting)
            {
                StartCoroutine(Attack());
            }
        }

    }

    protected override float getHorizontalDirection()
    {
        // If you see the player, walk toward the player (but only up until your shooting range). 
        // Otherwise, just patrol normally. 
        Vector2 playerPosition = PlayerChain.Instance.transform.position;
        Vector2 position = this.transform.position;

        float distanceXToPlayer = Mathf.Abs(playerPosition.x - position.x);
        float distanceYToPlayer = Mathf.Abs(playerPosition.y - position.y);

        if (PlayerChain.Instance.players.Count > 0 && 
            distanceXToPlayer < visionRange &&
            distanceYToPlayer < visionRange)
        {
            patrolling = false;
            float direction = (playerPosition.x - position.x) / (Mathf.Abs(playerPosition.x - position.x));

            // Stop if you're on a ledge or if you're within shooting range
            if (distanceXToPlayer < shootingRange && distanceYToPlayer < shootingRange)
            {
                return 0;
            }

            bool leftGround;
            bool rightGround;
            PhysicsUtl.LedgeCheck(collider2D, out leftGround, out rightGround);

            if (direction < 0 && !leftGround)
                return 0;
            else if (direction > 0 && !rightGround)
                return 0;

            return direction; 
        }
        else
        {
            patrolling = true;
            return base.getHorizontalDirection();
        }
    }

    IEnumerator Attack()
    {
        currentlyShooting = true;
        // Attack animation and sound
        //animator.SetTrigger("attack");
        AudioSource.PlayClipAtPoint(attackSoundEffect, new Vector3(0, 0, 0));

        if (weaponPrefab.GetCastTime() > 0)
            yield return new WaitForSeconds(weaponPrefab.GetCastTime());

        int weaponDirection = spriteRenderer.flipX ? -1 : 1;
        if (isBackwards) weaponDirection *= -1;
        Vector3 weaponWorldPosition = weaponSpawnPoint.position;

        Vector3 weaponLocalPosition = transform.InverseTransformPoint(weaponWorldPosition);//new Vector3(transform.position.x + weaponDirection * 0.5f, transform.position.y + 0.5f, transform.position.z);
        if (spriteRenderer.flipX) weaponLocalPosition.x *= -1;
        if (isBackwards) weaponLocalPosition.x *= -1; 

        weaponWorldPosition = transform.TransformPoint(weaponLocalPosition);
        Castable weapon = Instantiate(weaponPrefab, weaponWorldPosition, Quaternion.identity);
        //weapon.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");


        if (weaponPrefab is Bullet)
        {
            Bullet bullet = (Bullet)weapon;
            bullet.direction = weaponDirection;
        }

        yield return new WaitForSeconds(1f);

        currentlyShooting = false;
    }

}
