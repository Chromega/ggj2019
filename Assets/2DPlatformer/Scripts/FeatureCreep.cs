using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureCreep : MonoBehaviour
{
    public SpriteRenderer shell;
    public SpriteRenderer mainRenderer;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");
    }

    private void LateUpdate()
    {
        shell.flipX = mainRenderer.flipX;
    }
}
