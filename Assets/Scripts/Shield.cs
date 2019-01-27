using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Castable
{
    PlayerController pc;
    public override CastType GetCastType()
    {
        return CastType.Sustained;
    }

    public override void StartCast(PlayerController pc)
    {
        base.StartCast(pc);
        transform.parent = pc.transform;
        this.pc = pc;
    }

    private void Update()
    {
        if (pc)
        {
            Vector3 localPos = transform.localPosition;
            if (pc.spriteRenderer.flipX ^ localPos.x<0)
            {
                localPos.x *= -1;
                transform.localPosition = localPos;
            }
        }
    }

    public override void EndCast()
    {
        base.EndCast();
        Destroy(gameObject);
    }
}
