﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // strings
    public Canvas dialogueCanvas;
    string startText = "The story of a founder building the {0} for {1}";
    public string loseTextNoMoney = "You ran out of money. Start a new company?";
    public string loseTextNoPlayers = "Everyone quit. Start a new company?";
    public string memberLostText = "Team member burned out";
    public string winText = "You IPO'd! Start a new company?";
    public Text fundsLeftText;
    
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

    // state
    private bool isActive = true;
    public int funds = 10;

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to events
        PlayerChain.memberLost += PlayerChain_MemberLost;
        HealthableMonster.onDied += HealthableMonster_OnDied;

        updateFundsLeft(funds);
    }

    void PlayerChain_MemberLost()
    {
        funds -= 3;
        updateFundsLeft(funds);

        if (funds <= 0)
        {
            updateDialogueText(loseTextNoMoney, 999.0f);
            isActive = false;
        }
        else if (PlayerChain.Instance.players.Count <= 0)
        {
            updateDialogueText(loseTextNoPlayers, 999.0f);
            isActive = false;
        }
        else
        {
            updateDialogueText(memberLostText);
        }
    }


    void HealthableMonster_OnDied()
    {
        updateDialogueText(winText, 999.0f);
        isActive = false;
    }

    private void OnDestroy()
    {
        // unsubscribe from events
        PlayerChain.memberLost -= PlayerChain_MemberLost;
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

    public void updateFundsLeft(int hp)
    {
        fundsLeftText.text = "Funds Left: $" + funds.ToString();
    }

    void updateDialogueText(string text, float timeToDisplay = -1)
    {
        if (timeToDisplay > -1)
        {
            dialogueCanvas.GetComponent<DialogueController>().ShowText(text, timeToDisplay);
        }
        else dialogueCanvas.GetComponent<DialogueController>().ShowText(text);
    }
}
