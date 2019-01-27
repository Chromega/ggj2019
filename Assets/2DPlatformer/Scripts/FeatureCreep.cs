using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureCreep : MonoBehaviour
{
    public SpriteRenderer shell;
    public SpriteRenderer mainRenderer;
    public Castable weaponPrefab;
    public Transform weaponSpawnPointSmall;
    public Transform weaponSpawnPointBig;
    public AudioClip attackSoundEffect;

    bool isBackwards = true;

    float timer;

    enum State
    {
        Small,
        Big,
        Mega
    }
    State state;

    public Animator animator;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        StartCoroutine(AttackLoop());

        SetState(State.Small);
        yield return new WaitForSeconds(5f);
        SetState(State.Big);
        yield return new WaitForSeconds(5f);
        SetState(State.Mega);
    }

    void SetState(State state)
    {
        switch(state)
        {
            case State.Small:
                animator.SetBool("big", false);
                shell.gameObject.SetActive(false);
                break;
            case State.Big:
                animator.SetBool("big", true);
                shell.gameObject.SetActive(false);
                break;
            case State.Mega:
                animator.SetBool("big", true);
                shell.gameObject.SetActive(true);
                break;

        }
        this.state = state;
    }

    private void Update()
    {
        if (mainRenderer.isVisible)
        {
            timer += Time.deltaTime;

            switch (state)
            {
                case State.Small:
                    if (timer > 5)
                        SetState(State.Big);
                    break;
                case State.Big:
                    if (timer > 10)
                        SetState(State.Mega);
                    break;
            }

        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Attack());
        }*/
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            if (mainRenderer.isVisible)
            {
                StartCoroutine(Attack());
            }
            yield return new WaitForSeconds(Random.Range(2f, 3f));
        }
    }

    IEnumerator Attack()
    {
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(.5f);

        int weaponDirection = mainRenderer.flipX ? -1 : 1;
        if (isBackwards) weaponDirection *= -1;
        Vector3 weaponWorldPosition = state==State.Small?weaponSpawnPointSmall.position:weaponSpawnPointBig.position;

        Vector3 weaponLocalPosition = transform.InverseTransformPoint(weaponWorldPosition);//new Vector3(transform.position.x + weaponDirection * 0.5f, transform.position.y + 0.5f, transform.position.z);
        if (mainRenderer.flipX) weaponLocalPosition.x *= -1;
        //if (isBackwards) weaponLocalPosition.x *= -1;

        weaponWorldPosition = transform.TransformPoint(weaponLocalPosition);
        Castable weapon = Instantiate(weaponPrefab, weaponWorldPosition, Quaternion.identity);
        //weapon.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");


        if (weaponPrefab is Bullet)
        {
            Bullet bullet = (Bullet)weapon;
            bullet.direction = weaponDirection;
        }

        if (attackSoundEffect) AudioSource.PlayClipAtPoint(attackSoundEffect, new Vector3(0, 0, 0));
    }

    private void LateUpdate()
    {
        shell.flipX = mainRenderer.flipX;
    }
}
