using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea]
    public string text;
    uint dialogueHandle;
    bool hasTriggered = false;
    float minShowTime = 4f;
    float collisionTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered)
            return;
        hasTriggered = true;
        if (DialogueController.Instance)
        {
            if (other.gameObject.layer == (int)PhysicsUtl.LayerMasks.Player)
            {
                dialogueHandle = DialogueController.Instance.ShowText(text, 0);
                collisionTime = Time.realtimeSinceStartup;
            }
        }
        else
        {
            Debug.LogError("Null dialogue controller");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (DialogueController.Instance)
        {
            if (other.gameObject.layer == (int)PhysicsUtl.LayerMasks.Player)
                DialogueController.Instance.HideText(dialogueHandle, Mathf.Max(0, minShowTime - (Time.realtimeSinceStartup-collisionTime)));
        }
        else
        {
            Debug.LogError("Null dialogue controller");
        }
    }
}
