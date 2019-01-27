using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCredits : MonoBehaviour
{
    public RectTransform childTransform;
    public float speedMultiplier = 0.1f;
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Vector2 pivot = childTransform.pivot;
            pivot.y -= Time.deltaTime * speedMultiplier;
            if (pivot.y > 0)
            {
                childTransform.pivot = pivot;
            }
        }
    }
}
