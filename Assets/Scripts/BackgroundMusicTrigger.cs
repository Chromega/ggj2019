using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (BackgroundMusicController.Instance)
        {
            if (other.gameObject.layer == (int)PhysicsUtl.LayerMasks.Player)
            {
                BackgroundMusicController.Instance.ToggleFastBgMusic();
            }
        }
        else
        {
            Debug.LogError("Null background music controller");
        }
    }
}
