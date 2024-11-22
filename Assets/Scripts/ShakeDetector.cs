using UnityEngine;
using UnityEngine.UI;


public class ShakeDetector : MonoBehaviour
{
   private Animator shakerAnimator;
   public float shakeThreshold = 2.5f; // Sensitivity
   public float cooldownTime = 1.0f; // Time before another shake can trigger
   private float lastShakeTime;


   public Slider shakeProgressBar; // Reference to the UI Slider
   public int shakesToComplete = 3; // Total shakes needed to complete
   private int currentShakes = 0; // Tracks the number of shakes detected


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


       if (shakeProgressBar == null)
       {
           Debug.LogError("Shake Progress Bar not assigned!");
       }
       else
       {
           shakeProgressBar.maxValue = shakesToComplete;
           shakeProgressBar.value = currentShakes;
       }
   }


   void Update()
   {
       #if UNITY_EDITOR
       // Simulate a shake in the Unity Editor (e.g., press the Space key)
       if (Input.GetKeyDown(KeyCode.Space))
       {
           Debug.Log("Space bar pressed in Editor");
           DetectShake();
       }
       #else
       // Actual accelerometer logic for mobile devices
       Vector3 acceleration = Input.acceleration;
       float accelerationMagnitude = acceleration.sqrMagnitude;


       if (accelerationMagnitude > shakeThreshold && Time.time - lastShakeTime > cooldownTime)
       {
           Debug.Log("Shake detected on mobile");
           DetectShake();
           lastShakeTime = Time.time;
       }
       #endif
   }


   private void DetectShake()
   {
       if (shakerAnimator != null)
       {
           shakerAnimator.SetTrigger("Shake");
           Debug.Log("Shake trigger set!");


           currentShakes++;
           UpdateProgressBar();


           if (currentShakes >= shakesToComplete)
           {
               EndShakingScene();
           }
       }
       else
       {
           Debug.LogError("Animator not assigned or found!");
       }
   }


   private void UpdateProgressBar()
   {
       if (shakeProgressBar != null)
       {
           shakeProgressBar.value = currentShakes;
           Debug.Log($"Progress updated: {currentShakes}/{shakesToComplete}");
       }
   }


   private void EndShakingScene()
   {
       Debug.Log("Shaking scene completed!");
       // transition to pouring scene
       
   }
}



