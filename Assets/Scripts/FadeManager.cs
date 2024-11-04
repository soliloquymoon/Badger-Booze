using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    // Start is called before the first frame update

    private float fadeSpeed = 0.25f;
    
    private void Start() {
    //Grab the only CanvasGroup in the scene    
    fadeGroup = FindObjectOfType<CanvasGroup>();

    //Start with a black screen
    fadeGroup.alpha = 1;  
    }

    // Update is called once per frame
    void Update()
    {
        //Fade-in
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeSpeed;
    }
}
