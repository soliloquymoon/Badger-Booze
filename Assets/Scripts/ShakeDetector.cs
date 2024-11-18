using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    private Animator shakerAnimator; // Reference to the Animator component
    public float shakeThreshold = 2.5f; // Adjust sensitivity (higher = less sensitive)
    public float cooldownTime = 1.0f; // Time before another shake can trigger

    private float lastShakeTime;

    private void Start()
    {
        // Automatically find the Animator component on the same GameObject
        shakerAnimator = GetComponent<Animator>();

        if (shakerAnimator == null)
        {
            Debug.LogError("No Animator component found on this GameObject!");
        }
    }

    void Update()
    {
        // Get the device's acceleration
        Vector3 acceleration = Input.acceleration;

        // Calculate the magnitude of the acceleration
        float accelerationMagnitude = acceleration.sqrMagnitude;

        // If the magnitude exceeds the threshold and cooldown has passed
        if (accelerationMagnitude > shakeThreshold && Time.time - lastShakeTime > cooldownTime)
        {
            TriggerShakeAnimation();
            lastShakeTime = Time.time; // Reset the cooldown
        }
    }

    private void TriggerShakeAnimation()
    {
        if (shakerAnimator != null)
        {
            // Set the Shake trigger in the Animator
            shakerAnimator.SetTrigger("Shake");
        }
    }
}
