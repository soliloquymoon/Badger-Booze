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
    private GameState gameState;
    private float fadeSpeed = 0.25f;

    private bool recipeIsOn = false;
    private bool ingredientsIsOn = false;
    
    private void Start() {
    //Grab the only CanvasGroup in the scene    
    //fadeGroup = FindObjectOfType<CanvasGroup>();
    fadeGroup = GameObject.Find("Fade").GetComponent<CanvasGroup>();

    //
    dimFadeGroup = GameObject.Find("Dim").GetComponent<CanvasGroup>();

    // Get Game State
    gameState = GameObject.Find("GameState").GetComponent<GameState>();

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
                settingsPanel.SetActive(true);
                dimFadeGroup.alpha = 0.8F;
                AudioManager.Instance.PlaySFX("ButtonClick");
                gameState.StopTimer(); // Pause game
            }
            
        } else {
            settingsPanel.SetActive(false);
            dimFadeGroup.alpha = 0;
            AudioManager.Instance.PlaySFX("ButtonClick");
            gameState.StartTimer(); // Continue game
        }
    }

    public void OnExitSettingsClick() {
        Debug.Log("Settings exit button clicked, settings window closed.");
        settingsPanel.SetActive(false);
        dimFadeGroup.alpha = 0;
        gameState.StartTimer(); // Continue game
    }

    public void OnRecipeClick() {
        Debug.Log("Recipe Book Button clicked. Enter Recipe Window.");
        if(recipeBookPanel.activeSelf == false) {
            if(ingredientListPanel.activeSelf == true || settingsPanel.activeSelf == true) {
                Debug.Log("Other windows have to be closed first before opening Recipe window.");
                //TODO Implement a warning message UI for this condition
            } else {
                recipeIsOn = true;
                StartCoroutine(TurnOnRecipeBook(0.25f));
            }
        } else {
            recipeIsOn = false;
            StartCoroutine(TurnOffRecipeBook(0.25f));
        }

    }

    IEnumerator TurnOnRecipeBook(float duration) {
        Debug.Log("Recipe Book Opened");

        //play audio clip sfx
        AudioManager.Instance.PlaySFX("BookOpen");


        //wait
        yield return new WaitForSeconds(duration);

        recipeBookPanel.SetActive(true);
        dimFadeGroup.alpha = 0.8F;
    }

    IEnumerator TurnOffRecipeBook(float duration) {
        Debug.Log("Recipe Book Closed");

        //play audio clip sfx
        AudioManager.Instance.PlaySFX("BookOpen");


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
                ingredientsIsOn = true;
                if (ingredientsIsOn) {
                    StartCoroutine(TurnOnIngredients(0.25f));
                }
            }
            
        } else {
            ingredientsIsOn = false;
            if (!ingredientsIsOn) {
                StartCoroutine(TurnOffIngredients(0.25f));
            }
        }
    }

    IEnumerator TurnOnIngredients(float duration) {
        Debug.Log("Ingredients Opened");

        //play audio clip sfx
        AudioManager.Instance.PlaySFX("IngredientsOpen");


        //wait
        yield return new WaitForSeconds(duration);

        ingredientListPanel.SetActive(true);
        dimFadeGroup.alpha = 0.8F;
    }

    IEnumerator TurnOffIngredients(float duration) {
        Debug.Log("Ingredients Closed");

        //play audio clip sfx
        AudioManager.Instance.PlaySFX("IngredientsOpen");


        //wait
        yield return new WaitForSeconds(duration);

        ingredientListPanel.SetActive(false);
        dimFadeGroup.alpha = 0;
    }
}
