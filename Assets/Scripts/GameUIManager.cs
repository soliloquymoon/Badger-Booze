using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    
    private CanvasGroup fadeGroup;
    // Start is called before the first frame update

    public GameObject recipeBookPanel;
    public GameObject settingsPanel;
    public GameObject ingredientListPanel;
    private GameState gameState;
    private float fadeSpeed = 0.25f;

    public GameObject closeWindowButton;
    
    private void Start() {
        //Grab the only CanvasGroup in the scene    
        //fadeGroup = FindObjectOfType<CanvasGroup>();
        fadeGroup = GameObject.Find("Fade").GetComponent<CanvasGroup>();

        // Get Game State
        gameState = GameObject.Find("GameState").GetComponent<GameState>();

        //Start with a black screen
        fadeGroup.alpha = 1;  

        // deactive closeWindowButton
        closeWindowButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Fade-in
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeSpeed;
    }

    //Button Manager
     public void OnSettingsClick() {
        if(settingsPanel.activeSelf == false) {
            settingsPanel.SetActive(true);
            closeWindowButton.SetActive(true);
            AudioManager.Instance.PlaySFX("ButtonClick");
            gameState.StopTimer(); // Pause game
        } else {
            settingsPanel.SetActive(false);
            closeWindowButton.SetActive(false);
            AudioManager.Instance.PlaySFX("ButtonClick");
            gameState.StartTimer(); // Continue game
        }
    }

    public void OnExitSettingsClick() {
        settingsPanel.SetActive(false);
        closeWindowButton.SetActive(false);
        gameState.StartTimer(); // Continue game
    }

    public void OnRecipeClick() {
        StartCoroutine(TurnOnRecipeBook(0.25f));
    }

    IEnumerator TurnOnRecipeBook(float duration) {
        //play audio clip sfx
        AudioManager.Instance.PlaySFX("BookOpen");

        //wait
        yield return new WaitForSeconds(duration);

        recipeBookPanel.SetActive(true);
        closeWindowButton.SetActive(true);
    }

    IEnumerator TurnOffRecipeBook(float duration) {
        //play audio clip sfx
        AudioManager.Instance.PlaySFX("BookOpen");

        //wait
        yield return new WaitForSeconds(duration);

        recipeBookPanel.SetActive(false);
        closeWindowButton.SetActive(false);
    }

    public void OnIngredientsClick() {
        StartCoroutine(TurnOnIngredients(0.25f));
    }

    IEnumerator TurnOnIngredients(float duration) {
        //play audio clip sfx
        AudioManager.Instance.PlaySFX("IngredientsOpen");

        //wait
        yield return new WaitForSeconds(duration);

        ingredientListPanel.SetActive(true);
        closeWindowButton.SetActive(true);
    }

    IEnumerator TurnOffIngredients(float duration) {
        //play audio clip sfx
        AudioManager.Instance.PlaySFX("IngredientsOpen");

        //wait
        yield return new WaitForSeconds(duration);

        ingredientListPanel.SetActive(false);
        closeWindowButton.SetActive(false);
    }

    /*
     * TurnOffActiveWindow:
     * This function handles the behavior of CloseWindowButton. This button
     * automatically becomes active when a window is open and inactivates other
     * UI buttons. If clicked, it turns off the window that is currently active.
     */
    public void TurnOffActiveWindow() {
        if (settingsPanel.activeSelf == true)
            OnExitSettingsClick();
        if (ingredientListPanel.activeSelf == true)
            StartCoroutine(TurnOffIngredients(0.25f));
        if (recipeBookPanel.activeSelf == true)
            StartCoroutine(TurnOffRecipeBook(0.25f));
    }
}
