using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShakeDetector : MonoBehaviour
{
    private Animator shakerAnimator;
    public float shakeThreshold = 2.5f; // Sensitivity for shake detection
    public float cooldownTime = 1.0f; // Cooldown between actions
    private float lastActionTime;

    public Slider shakeProgressBar; // Slider for shaking progress
    public Slider tiltProgressBar; // Slider for tilting progress (child of cocktailImage)

    public int shakesToComplete = 3; // Total shakes needed
    public int tiltsToComplete = 3; // Total tilts needed
    private int currentShakes = 0; // Current shakes detected
    private int currentTilts = 0; // Current tilts detected

    public GameObject cocktailImage; // Reference to the cocktail image UI (parent of tiltProgressBar)

    public GameObject serveButton;

    public GameObject bartendingScene;
    public GameObject customerScene;
    public ShakerLid shakerLid;
    private Customer customer;
    private GameState gameState;
    private DrinkManager drinksManager;
    public GameObject openShaker;
    private Sprite[] drinkSprites;
    public GameObject shakeMessage;
    public GameObject tiltMessage;
    private bool isPouring = false;
    private bool isShaking = true;
    

    void Start()
    {
        // Initialize customer, drinks, and game state
        customer = GameObject.FindGameObjectWithTag("Customer").GetComponent<Customer>();
        customerScene = GameObject.FindGameObjectWithTag("CustomerScene");
        drinksManager = GameObject.FindGameObjectWithTag("DrinksManager").GetComponent<DrinkManager>();
        drinkSprites = Resources.LoadAll<Sprite>("Drinks");
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();

        
        Input.gyro.enabled = true;

        // Assign animator
        shakerAnimator = GetComponent<Animator>();
        if (shakerAnimator == null)
        {
            Debug.LogError("Animator not found on this GameObject!");
        }

        // Initialize progress bars
        if (shakeProgressBar != null)
        {
            shakeProgressBar.maxValue = shakesToComplete;
            shakeProgressBar.value = 0;
        }
        else
        {
            Debug.LogError("Shake Progress Bar not assigned!");
        }

        if (tiltProgressBar != null)
        {
            tiltProgressBar.maxValue = tiltsToComplete;
            tiltProgressBar.value = 0;
        }
        else
        {
            Debug.LogError("Tilt Progress Bar not assigned!");
        }

        // Hide cocktail image and tilt progress bar at the start
        if (cocktailImage != null)
        {
            cocktailImage.SetActive(false);
        }

        // Ensure the Serve Button is hidden initially
        if (serveButton != null)
        {
            serveButton.SetActive(false);
        }
        else
        {
            Debug.LogError("Serve Button reference is missing!");
        }
    }

    void Update()
    {
        #if UNITY_EDITOR
        // Simulate a shake in the editor (press Space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetectShake();
        }

        // Simulate a tilt in the editor (press P key)
        if (Input.GetKeyDown(KeyCode.P))
        {
            DetectTilt();
        }

        #else
        // Accelerometer-based shake detection
        Vector3 acceleration = Input.acceleration;
        float accelerationMagnitude = acceleration.sqrMagnitude;

        if (isShaking) {
            if (accelerationMagnitude > shakeThreshold && Time.time - lastActionTime > cooldownTime)
            {
                DetectShake();
                lastActionTime = Time.time;
            }   

        } 
        if (isPouring) {
            // Gyroscope-based tilt detection
            Vector3 gyroRotation = Input.gyro.rotationRateUnbiased;
            if (Mathf.Abs(gyroRotation.x) > 1.5f && Time.time - lastActionTime > cooldownTime)
            {
                DetectTilt();
                lastActionTime = Time.time;
            }
        }
        
        #endif
    }

    public void ServeDrink()
    {
        // Load the next scene
        if (bartendingScene != null)
        {
            bartendingScene.transform.SetSiblingIndex(0);
        }

        if (customerScene != null)
        {
            serveButton.SetActive(false);
        }

        if (cocktailImage != null)
        {
            cocktailImage.SetActive(true);
            tiltProgressBar.gameObject.SetActive(false);
            StartCoroutine(serveDrinkSFX(0.5f));
            


            // Set its position to the bottom middle of the screen
            RectTransform rectTransform = cocktailImage.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(0.5f, 0); // Anchor to the bottom middle
                rectTransform.anchorMax = new Vector2(0.5f, 0);
                rectTransform.pivot = new Vector2(0.5f, 0);    // Set pivot to the bottom center
                rectTransform.anchoredPosition = Vector2.zero;
                // Add 10% of Cocktail glass height to the Y value
                rectTransform.anchoredPosition += new Vector2(0, rectTransform.rect.height * 0.1f); 
            }
            // Scale the image down
            cocktailImage.transform.localScale = new Vector3(0.5f, 0.5f, 1f); // Scale down to 50%

            // Trigger drink served logic
            cocktailImage.transform.SetParent(customerScene.transform);
            gameState.DrinkServed();
            openShaker.SetActive(true);
            shakerLid.Reset();
        }
        else
        {
            Debug.LogError("Cocktail Image reference is missing!");
        }
    }

    IEnumerator serveDrinkSFX(float duration) {
        AudioManager.Instance.PlaySFX("Serve");
        yield return new WaitForSeconds(duration);
    }


    public void ResetBartendingScene() {
        openShaker.SetActive(true);
        shakerLid.Reset();
        isShaking = true;
        isPouring = false;
    }

    private void DetectShake()
    {
        if (shakerAnimator != null)
        {
            shakerAnimator.SetTrigger("Shake");
        }

        AudioManager.Instance.PlaySFX("Shake");

        currentShakes++;
        UpdateProgressBar(shakeProgressBar, currentShakes, shakesToComplete);

        if (currentShakes >= shakesToComplete)
        {
            MessageManager.Instance.TurnOffShakeMessage();
            MessageManager.Instance.TurnOnTiltMessage();
            EndShakingScene();

        }
    }

    private void DetectTilt()
    {
        if (shakerAnimator != null)
        {
            shakerAnimator.SetTrigger("Tilt");
        }

        AudioManager.Instance.PlaySFX("Pour");

        currentTilts++;
        UpdateProgressBar(tiltProgressBar, currentTilts, tiltsToComplete);

        if (currentTilts >= tiltsToComplete)
        {
            MessageManager.Instance.TurnOffTiltMessage();
            ShowCocktailFinalState();
        }
    }

    private void UpdateProgressBar(Slider progressBar, int currentValue, int totalValue)
    {
        if (progressBar != null)
        {
            progressBar.value = currentValue;
        }
    }

    private void EndShakingScene()
{
    isShaking = false;
    isPouring = true;


    // Display the cocktail image
    if (cocktailImage != null)
    {
        Drink mixingDrink = customer.GetMixingDrink();
        List<Drink> allDrinks = drinksManager.getDrinkList();

        // Find the closest score over 30 to get the closest drink to the mixing drink
        int maxScore = 0;
        Drink closestDrink = null;
        foreach (Drink drink in allDrinks) {
            int score = drinksManager.calculateScore(mixingDrink, drink);
            if(score >= 30) {
               maxScore = Mathf.Max(maxScore, score); 
               closestDrink = drink;
            }
        }

        // If it didn't match anything by over 30 score, assign a default sprite
        if(closestDrink == null) {
            cocktailImage.GetComponent<Image>().sprite = drinkSprites[3]; // Use Margarita sprite by default
        } else {
            // Find the sprite of the closest drink to assign
            string formattedName = closestDrink.getName().Replace(" ", "_");
            foreach (Sprite drinkSprite in drinkSprites) {
                if(drinkSprite.name.Equals(formattedName)) {
                    cocktailImage.GetComponent<Image>().sprite = drinkSprite;
                }
            }
        }

        cocktailImage.SetActive(true);
    }
    else
    {
        Debug.LogError("Cocktail Image reference is missing!");
    }

    // Ensure the tilt progress bar is explicitly set active
    if (tiltProgressBar != null)
    {
        tiltProgressBar.gameObject.SetActive(true); // Explicitly activate the progress bar
    }
    else
    {
        Debug.LogError("Tilt Progress Bar reference is missing!");
    }
}


    private void ShowCocktailFinalState()
    {
        isPouring = false;
        

        // Display the Serve Button
        if (serveButton != null)
        {
            serveButton.SetActive(true);
        }
        else
        {
            Debug.LogError("Serve Button reference is missing!");
        }
    }
}
