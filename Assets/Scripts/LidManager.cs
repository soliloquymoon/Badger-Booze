using UnityEngine;

public class LidManager : MonoBehaviour
{
    public Transform shaker;        // Reference to the Shaker GameObject (assign in Inspector)
    private Animator lidAnimator;

    void Start()
    {
        // Get the Lid's Animator
        lidAnimator = GetComponent<Animator>();
        if (lidAnimator == null)
        {
            Debug.LogError("Animator not found on this Lid GameObject!");
        }
    }

    public void TriggerLidShake()
    {
        // Check if the lid is attached at position (0, 0, 0) relative to the shaker
        if (IsLidAttached())
        {
            if (lidAnimator != null)
            {
                lidAnimator.SetTrigger("Shake");
                Debug.Log("Lid shake triggered!");
            }
        }
        else
        {
            Debug.LogWarning("Lid is not attached to the shaker!");
        }
    }

    private bool IsLidAttached()
    {
        // Check if the lid's local position is (0, 0, 0) relative to the shaker
        return transform.localPosition == Vector3.zero;
    }
}
