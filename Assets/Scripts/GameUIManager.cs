using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    
    private CanvasGroup fadeGroup;
    // Start is called before the first frame update

    private CanvasGroup dimFadeGroup;

    public GameObject recipeBookPanel;
    public GameObject settingsPanel;
    public GameObject ingredientListPanel;
    private float fadeSpeed = 0.25f;

    private bool recipeIsOn = false;

    public AudioSource source;
    public AudioClip bookWindowSound;
    public AudioClip ingredientWindowSound;
    public AudioClip buttonPressSound;
    
    private void Start() {
    //Grab the only CanvasGroup in the scene    
    //fadeGroup = FindObjectOfType<CanvasGroup>();
    fadeGroup = GameObject.Find("Fade").GetComponent<CanvasGroup>();

    //
    dimFadeGroup = GameObject.Find("Dim").GetComponent<CanvasGroup>();

    //Start with a black screen
    fadeGroup.alpha = 1;  

    //start dimFadeGroup alpha with 0
    dimFadeGroup.alpha = 0;
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
        if(settingsPanel.activeSelf == false) {
            if(recipeBookPanel.activeSelf == true || ingredientListPanel.activeSelf == true || recipeIsOn == true) {
                Debug.Log("Other windows have to be closed first before opening Settings window.");
                //TODO Implement a warning message UI for this condition
            } else {
                //StartCoroutine(TurnOnRecipeBook(0.35f));
                settingsPanel.SetActive(true);
                dimFadeGroup.alpha = 0.85F;
            }
            
        } else {
            //StartCoroutine(TurnOffRecipeBook(0.35f));
            settingsPanel.SetActive(false);
            dimFadeGroup.alpha = 0;

        }
    }

    public void OnRecipeClick() {
        Debug.Log("Recipe Book Button clicked. Enter Recipe Window.");
        if(recipeBookPanel.activeSelf == false) {
            if(ingredientListPanel.activeSelf == true || settingsPanel.activeSelf == true) {
                Debug.Log("Other windows have to be closed first before opening Recipe window.");
                //TODO Implement a warning message UI for this condition
            } else {
                recipeIsOn = true;
                StartCoroutine(TurnOnRecipeBook(0.35f));
            }
        } else {
            recipeIsOn = false;
            StartCoroutine(TurnOffRecipeBook(0.35f));
        }

    }

    IEnumerator TurnOnRecipeBook(float duration) {
        Debug.Log("Recipe Book Opened");

        //play audio clip sfx
        source.PlayOneShot(bookWindowSound);


        //wait
        yield return new WaitForSeconds(duration);

        recipeBookPanel.SetActive(true);
        dimFadeGroup.alpha = 0.85F;
    }

    IEnumerator TurnOffRecipeBook(float duration) {
        Debug.Log("Recipe Book Closed");

        //play audio clip sfx
        source.PlayOneShot(bookWindowSound);

        //wait
        yield return new WaitForSeconds(duration);

        recipeBookPanel.SetActive(false);
        dimFadeGroup.alpha = 0;
    }

    public void OnIngredientsClick() {
        Debug.Log("Ingredients Button clicked. Enter Ingredients Window.");
        if(ingredientListPanel.activeSelf == false) {
            if(recipeBookPanel.activeSelf == true || settingsPanel.activeSelf == true || recipeIsOn == true) {
                Debug.Log("Other windows have to be closed first before opening Ingredients window.");
                //TODO Implement a warning message UI for this condition
            } else {
                //StartCoroutine(TurnOnRecipeBook(0.35f));
                ingredientListPanel.SetActive(true);
                dimFadeGroup.alpha = 0.85F;
            }
            
        } else {
            //StartCoroutine(TurnOffRecipeBook(0.35f));
            ingredientListPanel.SetActive(false);
            dimFadeGroup.alpha = 0;

        }
    }
}
