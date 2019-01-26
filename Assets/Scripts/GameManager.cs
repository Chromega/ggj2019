using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Canvas dialogueCanvas;
    public string startText = "The story of a founder building the Uber for Crypto";

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.onDied += PlayerController_OnDied;
    }

    void PlayerController_OnDied()
    {
        dialogueCanvas.GetComponent<DialogueController>().ShowText("YOU DEAD", 6.0f);
    }

    private void OnDestroy()
    {
        PlayerController.onDied -= PlayerController_OnDied;
    }

    private void Awake()
    {
        if (!dialogueCanvas)
        {
            dialogueCanvas = FindObjectOfType(typeof(Canvas)) as Canvas;
        }

        if (!dialogueCanvas)
        {
            Debug.LogError("Please attach a canvas with a DialogueController object");
        }

        dialogueCanvas.GetComponent<DialogueController>().ShowText(startText, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
