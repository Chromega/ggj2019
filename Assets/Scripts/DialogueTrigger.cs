using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string text;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (DialogueController.Instance)
        {
            if (other.gameObject.layer == (int)PhysicsUtl.LayerMasks.Player)
                DialogueController.Instance.ShowText(text);
        }
        else
        {
            Debug.LogError("Null dialogue controller");
        }
    }
}
