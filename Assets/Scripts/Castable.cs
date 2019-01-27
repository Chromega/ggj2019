using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Castable : MonoBehaviour
{
    public enum CastType
    {
        Instant,
        Sustained
    }

    public abstract CastType GetCastType();

    public virtual void StartCast(PlayerController pc) { }
    public virtual void EndCast() {}
}
