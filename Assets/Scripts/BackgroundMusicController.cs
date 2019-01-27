using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioSource bgSlow;
    public AudioSource bgFast;

    public static BackgroundMusicController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void ToggleFastBgMusic()
    {
        StartCoroutine(AudioController.FadeOut(bgSlow, 1f));
        StartCoroutine(AudioController.FadeIn(bgFast, 1f));
    }
}
