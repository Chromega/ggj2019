using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas dialogueCanvas;
    public string startText = "The story of a founder building the Uber for Crypto";
    public string loseText = "You ran out of money. Start a new company?";
    public string winText = "You IPO'd! Start a new company?";
    private bool isActive = true;


    // Start is called before the first frame update
    void Start()
    {
        // subscribe to events
        PlayerController.onDied += PlayerController_OnDied;
        HealthableMonster.onDied += HealthableMonster_OnDied;
    }

    void HealthableMonster_OnDied()
    {
        dialogueCanvas.GetComponent<DialogueController>().ShowText(winText, 999.0f);
        isActive = false;
    }


    void PlayerController_OnDied()
    {
        dialogueCanvas.GetComponent<DialogueController>().ShowText(loseText, 999.0f);
        isActive = false;
    }

    private void OnDestroy()
    {
        // unsubscribe from events
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
        if (!isActive)
        {
            if (Input.GetButtonUp("Submit"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }


}
