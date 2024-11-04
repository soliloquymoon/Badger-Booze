using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float loadTime;
    private float minimumLogoTime = 4.0f; //Minimum time of that scene


    private void Start() {
    //Grab the only CanvasGroup in the scene    
    fadeGroup = FindObjectOfType<CanvasGroup>();

    //Start with a black screen
    fadeGroup.alpha = 1;  




    //Preload the game
    // TODO if any preload is needed here



    //Get a timestamp of the completion time
    //if loadtime is very fast, give it a small buffer time so the logo can still be viewed

    if (Time.time < minimumLogoTime)
        loadTime = minimumLogoTime;
    else
        loadTime = Time.time;
    }

    private void Update() {
        //Fade-in
        if (Time.time < minimumLogoTime)
        {
            fadeGroup.alpha = 1 - Time.time;
        }

        //Fade-out
        if(Time.time > minimumLogoTime && loadTime != 0) {
            fadeGroup.alpha = Time.time - minimumLogoTime;
            if (fadeGroup.alpha >= 1) {
                Debug.Log("Change the scene from preloader to menu scene");
                SceneManager.LoadScene("TitleScene");
            }
        }
    }

}
