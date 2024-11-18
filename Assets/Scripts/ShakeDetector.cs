using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    private Animator shakerAnimator;
    public float shakeThreshold = 2.5f; // Sensitivity
    public float cooldownTime = 1.0f; // Time before another shake can trigger
    private float lastShakeTime;

    void Start()
    {
        shakerAnimator = GetComponent<Animator>();
        if (shakerAnimator == null)
        {
            Debug.LogError("Animator not found on this GameObject!");
        }
        else
        {
            Debug.Log("Animator assigned successfully");
        }
    }

    void Update()
    {
        #if UNITY_EDITOR
        // Simulate a shake in the Unity Editor (e.g., press the Space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space bar pressed in Editor");
            TriggerShakeAnimation();
        }
        #else
        // Actual accelerometer logic for mobile devices
        Vector3 acceleration = Input.acceleration;
        float accelerationMagnitude = acceleration.sqrMagnitude;

        if (accelerationMagnitude > shakeThreshold && Time.time - lastShakeTime > cooldownTime)
        {
            Debug.Log("Shake detected on mobile");
            TriggerShakeAnimation();
            lastShakeTime = Time.time;
        }
        #endif
    }

    private void TriggerShakeAnimation()
    {
        if (shakerAnimator != null)
        {
            shakerAnimator.SetTrigger("Shake");
            Debug.Log("Shake trigger set!");
        }
        else
        {
            Debug.LogError("Animator not assigned or found!");
        }
    }
}
