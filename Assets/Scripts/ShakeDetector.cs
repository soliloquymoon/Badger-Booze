using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
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

        if (accelerationMagnitude > shakeThreshold && Time.time - lastActionTime > cooldownTime)
        {
            DetectShake();
            lastActionTime = Time.time;
        }

        // Gyroscope-based tilt detection
        Vector3 gyroRotation = Input.gyro.rotationRateUnbiased;
        if (Mathf.Abs(gyroRotation.x) > 1.5f && Time.time - lastActionTime > cooldownTime)
        {
            DetectTilt();
            lastActionTime = Time.time;
        }
        #endif
    }

    private void DetectShake()
    {
        if (shakerAnimator != null)
        {
            shakerAnimator.SetTrigger("Shake");
        }

        currentShakes++;
        UpdateProgressBar(shakeProgressBar, currentShakes, shakesToComplete);

        if (currentShakes >= shakesToComplete)
        {
            Debug.Log("Shaking completed!");
            EndShakingScene();
        }
    }

    private void DetectTilt()
    {
        currentTilts++;
        UpdateProgressBar(tiltProgressBar, currentTilts, tiltsToComplete);

        if (currentTilts >= tiltsToComplete)
        {
            Debug.Log("Tilting completed!");
            ShowCocktailFinalState();
        }
    }

    private void UpdateProgressBar(Slider progressBar, int currentValue, int totalValue)
    {
        if (progressBar != null)
        {
            progressBar.value = currentValue;
            Debug.Log($"Progress updated: {currentValue}/{totalValue}");
        }
    }

    private void EndShakingScene()
{
    Debug.Log("Shaking scene completed!");

    // Display the cocktail image
    if (cocktailImage != null)
    {
        cocktailImage.SetActive(true);
        Debug.Log("Cocktail Image displayed!");
    }
    else
    {
        Debug.LogError("Cocktail Image reference is missing!");
    }

    // Ensure the tilt progress bar is explicitly set active
    if (tiltProgressBar != null)
    {
        tiltProgressBar.gameObject.SetActive(true); // Explicitly activate the progress bar
        Debug.Log("Tilt Progress Bar activated!");
    }
    else
    {
        Debug.LogError("Tilt Progress Bar reference is missing!");
    }
}


    private void ShowCocktailFinalState()
    {
        Debug.Log("Cocktail completed! Add further logic for success state here.");
    }
}
