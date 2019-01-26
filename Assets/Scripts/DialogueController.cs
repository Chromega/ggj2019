﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public CanvasGroup group;
    public Text dialogueText;
    float targetFade = 0;

    public static DialogueController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        group.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        group.alpha = Mathf.MoveTowards(group.alpha, targetFade, 2f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowText("TEST!!!");
        }
    }

    public void ShowText(string text)
    {
        dialogueText.text = text;
        targetFade = 1f;
        StartCoroutine(HideTextCoroutine(2f));
    }

    IEnumerator HideTextCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        targetFade = 0;
    }
}
