using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas dialogueCanvas;
    string startText = "The story of a founder building the {0} for {1}";
    public string loseText = "You ran out of money. Start a new company?";
    public string winText = "You IPO'd! Start a new company?";
    private bool isActive = true;

    string[] companies =
    {
        "Uber",
        "AirBnB",
        "Facebook",
        "Twitter",
        "Tesla",
        "Spotify",
        "Pandora",
        "Apple",
        "Google",
        "Amazon",
        "Rover",
        "Lyft",
        "Waze",
        "Tumblr",
        "Reddit",
        "Salesforce"
    };

    string[] industries =
    {
        "horticulture",
        "katanas",
        "burritos",
        "birdwatching",
        "interior decorating",
        "stamp collecting",
        "citrus",
        "socks",
        "narcotics",
        "stretch marks",
        "toe hair"
    };

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
        HealthableMonster.onDied -= HealthableMonster_OnDied;
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

        string company = companies[Random.Range(0, companies.Length)];
        string industry = industries[Random.Range(0, industries.Length)];
        dialogueCanvas.GetComponent<DialogueController>().ShowText(string.Format(startText, company, industry), 6.0f);
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
