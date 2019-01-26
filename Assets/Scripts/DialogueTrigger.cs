using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string text;

    private void OnTriggerEnter(Collider other)
    {
        if (DialogueController.Instance)
        {
            DialogueController.Instance.ShowText(text);
        }
        else
        {
            Debug.LogError("Null dialogue controller");
        }
    }
}
