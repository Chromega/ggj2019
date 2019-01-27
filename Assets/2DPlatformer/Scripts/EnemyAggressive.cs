using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggressive : EnemyPatrol
{
    private bool jump = false;

    protected override bool getJump()
    {
        return jump;
    }

    protected override float getHorizontalDirection()
    {
        // If you see the player, walk toward the player. Otherwise, just patrol
        // normally. 
        Vector2 playerPosition = PlayerChain.Instance.transform.position;
        Vector2 position = this.transform.position;

        float distanceXToPlayer = Mathf.Abs(playerPosition.x - position.x);
        float distanceYToPlayer = Mathf.Abs(playerPosition.y - position.y);

        if (PlayerChain.Instance.players.Count > 0 &&
            distanceXToPlayer < visionRange &&
            distanceYToPlayer < 1)
        {
            patrolling = false; 
            float direction = (playerPosition.x - position.x) / (Mathf.Abs(playerPosition.x - position.x));

            // If on a ledge, yolo jump!
            bool leftGround;
            bool rightGround;
            PhysicsUtl.LedgeCheck(collider2D, out leftGround, out rightGround);

            if (direction < 0 && !leftGround)
            {
                jump = true;
            }
            else if (direction > 0 && !rightGround)
                jump = true;

            // Stop jumping (mimic keypress for the full jump parabola) 
           StartCoroutine(FinishJump());

            return direction;
        }
        else
        {
            patrolling = true;
            return base.getHorizontalDirection();
        }
    }

    IEnumerator FinishJump()
    {
        yield return new WaitForSeconds(1f);
        jump = false;
    }
}