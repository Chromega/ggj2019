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
    }

    private void Awake()
    {
        dialogueCanvas.GetComponent<DialogueController>().ShowText(startText, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
