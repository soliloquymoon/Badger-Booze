using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkIn : MonoBehaviour
{
    public float moveSpeed = 25f;           // Speed of horizontal movement
    public float bounceHeight = 4f;         // Height of each bounce
    public float bounceFrequency = 6f;      // Frequency of the bounce
    public GameObject dialogueBox;          // Reference to the dialogue box GameObject
    public GameObject customerMood;         // Reference to the customer mood GameObject
    public GameObject barBackground;        // Reference to the bar background GameObject

    private float startX;                   // Starting x position to reset when needed
    private float bounceTimer = 0f;         // Timer for bounce effect
    private float leftBound;                // Left boundary of the bar background
    private float rightBound;               // Right boundary of the bar background
    private float bottomThirdYPosition;     // The Y position for the bottom third of the scene
    private float lastThirdXPosition;       // The X position where the character stops (last third)
    private bool hasReachedMiddle = false;  // Flag to check if the character reached the middle

    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;     // Record the initial x position of the character

        // Get the bounds of the bar background (assuming it has a collider)
        if (barBackground != null)
        {
            BoxCollider2D boxCollider = barBackground.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                // Calculate left and right boundaries based on the collider's size
                leftBound = barBackground.transform.position.x - (boxCollider.size.x / 2);
                rightBound = barBackground.transform.position.x + (boxCollider.size.x / 2);

                // Calculate the bottom third y position
                float backgroundHeight = boxCollider.size.y;
                bottomThirdYPosition = barBackground.transform.position.y - (backgroundHeight / 3);

                // Calculate the position where the character should stop in the last third of the screen
                lastThirdXPosition = barBackground.transform.position.x + (boxCollider.size.x / 3);
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
        if (transform.position.x > lastThirdXPosition)
        {
            // Horizontal movement
            float newX = transform.position.x - (moveSpeed * Time.deltaTime);

            // Apply the new horizontal position, but stop if it reaches the last third
            if (newX <= lastThirdXPosition)
            {
                newX = lastThirdXPosition;
                hasReachedMiddle = true; // Set the flag to true when reaching the middle
            }

            transform.position = new Vector2(newX, transform.position.y);
        }
        else if (!hasReachedMiddle)
        {
            hasReachedMiddle = true; // Set the flag if not already set
        }

        // Display dialogue box and customer mood when in the last third
        if (hasReachedMiddle)
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
        if (!hasReachedMiddle)
        {
            bounceTimer += Time.deltaTime * bounceFrequency;
            float yOffset = Mathf.Sin(bounceTimer) * bounceHeight;
            transform.position = new Vector2(transform.position.x, bottomThirdYPosition + yOffset);
        }
        else
        {
            // Stop bouncing and set the y position to the fixed bottom third position
            transform.position = new Vector2(transform.position.x, bottomThirdYPosition);
        }
    }
}