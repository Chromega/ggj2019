using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyPatrol
{

    public float visionRange = 7;
    public float shootingRange = 5;
    public Transform weaponSpawnPoint;
    public AudioClip attackSoundEffect;
    public Castable weaponPrefab;

    private bool shooting = false;

    private void Start()
    {
        StartCoroutine(Cast());
    }

    protected override void Update()
    {
        base.Update();

        Vector2 playerPosition = PlayerChain.Instance.transform.position;
        Vector2 position = this.transform.position;

        float distanceToPlayer = Mathf.Abs(playerPosition.x - position.x);
        if (distanceToPlayer < shootingRange)
        {
            shooting = true;
        }
        else
        {
            shooting = false;
        }
    }

    protected override float getHorizontalDirection()
    {
        // If you see the player, walk toward the player (but only up until your shooting range). 
        // Otherwise, just patrol normally. 
        Vector2 playerPosition = PlayerChain.Instance.transform.position;
        Vector2 position = this.transform.position;

        float distanceToPlayer = Mathf.Abs(playerPosition.x - position.x);
        if (PlayerChain.Instance.players.Count > 0 && 
            distanceToPlayer < visionRange)
        {
            patrolling = false;
            float direction = (playerPosition.x - position.x) / (Mathf.Abs(playerPosition.x - position.x));

            // Stop if you're on a ledge or if you're within shooting range

            if (distanceToPlayer < shootingRange) return 0; 

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

    IEnumerator Cast()
    {
        while (shooting)
        {

            yield return new WaitForSeconds(1f);
            Debug.Log("SHOOTING");

            // Attack animation and sound
            //animator.SetTrigger("attack");
            //AudioSource.PlayClipAtPoint(attackSoundEffect, new Vector3(0, 0, 0));

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


            if (weaponPrefab is Bullet)
            {
                Debug.Log("bulleting");
                Bullet bullet = (Bullet)weapon;
                bullet.direction = weaponDirection;
            }
        }
    }
}
