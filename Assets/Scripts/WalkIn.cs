using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkIn : MonoBehaviour
{
    public float moveSpeed = 100f;           // Speed of horizontal movement
    public float bounceHeight = 4f;         // Height of each bounce
    public float bounceFrequency = 6f;      // Frequency of the bounce
    public GameObject dialogueBox;          // Reference to the dialogue box GameObject
    public GameObject customerMood;         // Reference to the customer mood GameObject
    public GameObject barBackground;        // Reference to the bar background GameObject
    private float bounceTimer = 0f;         // Timer for bounce effect
    private float bounceYPosition;     // The Y position for the character (1/3)
    private float targetXPosition;       // The X position where the character stops (3/4)
    private bool reachedPosition = false;  // Flag to check if the character reached the middle

    // Start is called before the first frame update
    void Start()
    {
        // Get the bounds of the bar background (assuming it has a collider)
        if (barBackground != null)
        {
            BoxCollider2D boxCollider = barBackground.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                float backgroundHeight = boxCollider.size.y;
                bounceYPosition = barBackground.transform.position.y - (backgroundHeight / 3);

                // Customer starts walking at the end of the screen
                transform.position = new Vector3(Screen.width, transform.position.y, transform.position.z);
                // Calculate the position where the character should stop (3/4 of the screen from the left)
                targetXPosition = Screen.width * 0.75f;
            }
        }

        // Find the dialogue box (assuming it's already assigned in Inspector)
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false); // Hide the dialogue box initially
        }

        if (customerMood != null)
        {
            customerMood.SetActive(false);  // Hide the customer mood initially
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the character has reached the last third of the screen on the x-axis
        if (!reachedPosition)
        {
            // Convert to world position, using the character’s current Y and Z to avoid overwriting those
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(targetXPosition, Screen.height / 2f, Camera.main.nearClipPlane));
            
            // Move the character towards the target position
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

            if(transform.position.x <= targetXPosition) {
                reachedPosition = true;
            }
        }

        // Display dialogue box and customer mood when in the last third
        if (reachedPosition)
        {
            if (dialogueBox != null)
            {
                dialogueBox.SetActive(true);
            }
            if (customerMood != null)
            {
                customerMood.SetActive(true);
            }
        }

        // Apply bounce effect only if not yet in the middle
        if (!reachedPosition)
        {
            bounceTimer += Time.deltaTime * bounceFrequency;
            float yOffset = Mathf.Sin(bounceTimer) * bounceHeight;
            transform.position = new Vector2(transform.position.x, bounceYPosition + yOffset);
        }
        else
        {
            // Stop bouncing and set the y position to the fixed bottom third position
            transform.position = new Vector2(transform.position.x, bounceYPosition);
        }
    }
}