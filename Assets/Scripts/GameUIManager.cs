using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    
    private CanvasGroup fadeGroup;
    // Start is called before the first frame update

    public GameObject recipeBookPanel;


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

    //Button Manager
    public void OnSettingsClick() {
        Debug.Log("Settings Button clicked. Enter Settings Window.");
        //TODO implement functionality and window
    }

    public void OnRecipeClick() {
        Debug.Log("Recipe Book Button clicked. Enter Recipe Window.");
        if(recipeBookPanel.activeSelf == false) {
            StartCoroutine(TurnOnRecipeBook(0.35f));
        } else {
            StartCoroutine(TurnOffRecipeBook(0.35f));

        }

    }

    IEnumerator TurnOnRecipeBook(float duration) {
        Debug.Log("Recipe Book Opened");

        //wait
        yield return new WaitForSeconds(duration);

        recipeBookPanel.SetActive(true);
    }

    IEnumerator TurnOffRecipeBook(float duration) {
        Debug.Log("Recipe Book Closed");

        //wait
        yield return new WaitForSeconds(duration);

        recipeBookPanel.SetActive(false);
    }

    public void OnIngredientsClick() {
        Debug.Log("Ingredients Button clicked. Enter Ingredients Window.");
                //TODO implement functionality and window

    }
}
