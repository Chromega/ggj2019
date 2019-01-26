using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsUtl
{
    public enum LayerMasksBitmasks
    {
        Default = 1<<0,
        Level = 1<<8,
        Enemy = 1<<9,
        Usable = 1<<10,
        Interactable=1<<11,
        IgnorePlayer=1<<12,
        Player=1<<13,
        Cameras=1<<14,
        LitSprites=1<<15,
        Destructable=1<<16,
        EnemyWeapon=1<<17,
        BossWeakPoint=1<<18,
        Bullet=1<<19
    }
    public enum LayerMasks
    {
        Default = 0,
        Level = 8,
        Enemy = 9,
        Usable = 10,
        Interactable = 11,
        IgnorePlayer = 12,
        Player = 13,
        Cameras = 14,
        LitSprites = 15,
        Destructable = 16,
        EnemyWeapon = 17,
        BossWeakPoint = 18,
        Bullet = 19
    }
    public static void LedgeCheck(Collider2D source, out bool leftGround, out bool rightGround)
    {
        Vector3 bottomLeft = source.bounds.min - .05f * Vector3.right;
        Vector3 bottomRight = source.bounds.center + new Vector3(source.bounds.extents.x, -source.bounds.extents.y, 0) + .05f * Vector3.right;

        leftGround = Physics2D.Linecast(bottomLeft, bottomLeft + .1f * Vector3.down, (int)LayerMasksBitmasks.Default);
        rightGround = Physics2D.Linecast(bottomRight, bottomRight + .1f * Vector3.down, (int)LayerMasksBitmasks.Default);
    }
}
