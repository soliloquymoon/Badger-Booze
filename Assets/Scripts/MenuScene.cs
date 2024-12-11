using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{

    private CanvasGroup fadeGroup;
    // Start is called before the first frame update

    private CanvasGroup dimFadeGroup;

    private float fadeInSpeed = 0.23f;
    private float fadeOutSpeed = 0.33f;

    private bool isStartingGame = false;

    private bool recipeIsOn = false;

    private bool ingredientsIsOn = false;
    private float startClickTimeStamp;
    public Button startButton;
    public GameObject recipeBookPanel;
    public GameObject ingredientListPanel;

    public GameObject settingsPanel;



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
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;
        
        if (isStartingGame == true) {
            fadeGroup.alpha += (Time.time - startClickTimeStamp) * fadeOutSpeed;

            if (fadeGroup.alpha >= 1) {
                SceneManager.LoadScene("GameScene");
            }
        }   
    }

    //Buttons
    public void OnStartClick() {
        AudioManager.Instance.PlaySFX("ButtonClick");
        isStartingGame = true;
        startClickTimeStamp = Time.time;
        startButton.interactable = false;
    }

    public void OnSettingsClick() {
        if(settingsPanel.activeSelf == false) {
            if(recipeBookPanel.activeSelf == true || ingredientListPanel.activeSelf == true || recipeIsOn == true) {
                Debug.Log("Other windows have to be closed first before opening Settings window.");
                //TODO Implement a warning message UI for this condition
            } else {
                //StartCoroutine(TurnOnRecipeBook(0.35f));
                settingsPanel.SetActive(true);
                dimFadeGroup.alpha = 0.8F;
                AudioManager.Instance.PlaySFX("ButtonClick");
            }
            
        } else {
            //StartCoroutine(TurnOffRecipeBook(0.35f));
            settingsPanel.SetActive(false);
            dimFadeGroup.alpha = 0;
            AudioManager.Instance.PlaySFX("ButtonClick");
        }
    }

    public void OnExitSettingsClick() {
        AudioManager.Instance.PlaySFX("ButtonClick");
        settingsPanel.SetActive(false);
        dimFadeGroup.alpha = 0;
    }

    public void OnRecipeClick() {
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

        //play audio clip sfx
        //source.PlayOneShot(bookWindowSound);
        AudioManager.Instance.PlaySFX("BookOpen");


        //wait
        yield return new WaitForSeconds(duration);

        recipeBookPanel.SetActive(true);
        dimFadeGroup.alpha = 0.8F;
    }

    IEnumerator TurnOffRecipeBook(float duration) {
        //play audio clip sfx
        AudioManager.Instance.PlaySFX("BookOpen");


        //wait
        yield return new WaitForSeconds(duration);

        recipeBookPanel.SetActive(false);
        dimFadeGroup.alpha = 0;
    }

    public void OnIngredientsClick() {
        if(ingredientListPanel.activeSelf == false) {
            if(recipeBookPanel.activeSelf == true || settingsPanel.activeSelf == true || recipeIsOn == true) {
                Debug.Log("Other windows have to be closed first before opening Ingredients window.");
                //TODO Implement a warning message UI for this condition
            } else {
                //StartCoroutine(TurnOnRecipeBook(0.35f));
                ingredientsIsOn = true;
                if (ingredientsIsOn) {
                    StartCoroutine(TurnOnIngredients(0.25f));
                }
            }
            
        } else {
            //StartCoroutine(TurnOffRecipeBook(0.35f));
            ingredientsIsOn = false;
            if (!ingredientsIsOn) {
                StartCoroutine(TurnOffIngredients(0.25f));
            }
        }
    }

    IEnumerator TurnOnIngredients(float duration) {

        //play audio clip sfx
        //source.PlayOneShot(bookWindowSound);
        AudioManager.Instance.PlaySFX("IngredientsOpen");


        //wait
        yield return new WaitForSeconds(duration);

        ingredientListPanel.SetActive(true);
        dimFadeGroup.alpha = 0.8F;
    }

    IEnumerator TurnOffIngredients(float duration) {

        //play audio clip sfx
        //source.PlayOneShot(bookWindowSound);
        AudioManager.Instance.PlaySFX("IngredientsOpen");


        //wait
        yield return new WaitForSeconds(duration);

        ingredientListPanel.SetActive(false);
        dimFadeGroup.alpha = 0;
    }

    //TODO Only for Play Mode purposes to check tutorial functionality, 
    //must delete later for actual build
    //void OnApplicationQuit() {
    //    PlayerPrefs.DeleteAll();
    //}

    
}
