using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    // Character movement
    private float initialYPosition;         // The Y position where the character starts
    private float targetXPosition;       // The X position where the character stops (3/4)
    private float targetYPosition;       // The Y position where the character stops (1/3 from the top)
    private bool reachedPosition = false;  // Flag to check if the character reached the middle
    public float moveSpeed = 20f;           // Speed of horizontal movement
    public float bounceHeight = 4f;         // Height of each bounce
    public float bounceFrequency = 6f;      // Frequency of the bounce
    private float bounceTimer = 0f;         // Timer for the bounce

    // Dialogue and Mood
    private GameObject dialogueBox;          // Reference to the dialogue box GameObject
    private Text dialogueText;
    private GameObject askButton;
    private GameObject customerMood;         // Reference to the customer mood GameObject
    private Text customerMoodText;
    private int moodScore;
    private float moodTimer = 0f;

    // Order Management
    private OrderManager orderManager;
    private Order currentOrder;

    // Badger Sprites
    private Sprite[] badgerSprites;
    private Image badgerImage;

    // Scenes and Game State
    private GameObject customerScene;
    private GameObject bartendingScene;
    private GameState gameState;

    // Drink being mixed for the customer
    private Drink mixingDrink;

    // Start is called before the first frame update
    void Start()
    {
        // Get Game State
        gameState = GameObject.Find("GameState").GetComponent<GameState>();

        // Customer starts walking at the end of the screen
        transform.position = new Vector3(Screen.width, Screen.height / 2f, transform.position.z);
        // Calculate the position where the character should stop (3/4 of the screen from the left)
        initialYPosition = transform.position.y;
        targetXPosition = Screen.width * 0.75f;
        targetYPosition = Screen.height / 2f;

        // Load the badger sprites
        badgerSprites = Resources.LoadAll<Sprite>("badgers");
        badgerImage = GetComponent<Image>();
        
        // Select from 8 different badger images
        int randomIndex = Random.Range(0, 7);
        badgerImage.sprite = badgerSprites[randomIndex];

        // Get Customer UIs
        askButton = GameObject.Find("AskButton");
        dialogueBox = GameObject.Find("Dialogue");
        dialogueText = GameObject.Find("DialogueText").GetComponent<Text>();

        customerMood = GameObject.Find("CustomerMood");
        customerMoodText = GameObject.Find("CustomerMoodText").GetComponent<Text>();
        moodScore = 100;

        // Initialize possible screens
        customerScene = GameObject.Find("CustomerScene");
        bartendingScene = GameObject.Find("BartendingScene");
            
        // Choose a drink order and a dialogue
        orderManager = new OrderManager();
        currentOrder = orderManager.generateCustomerOrder();
        SetDialogueText(currentOrder.getCustomerOrder());
        SetCustomerMoodText(moodScore + "%");

        customerMood.SetActive(false);  // Hide the customer mood initially
        dialogueBox.SetActive(false); // Hide the dialogue box initially

        // Initialize the mixing drink
        mixingDrink = new Drink();
    }

    // Update is called once per frame
    void Update()
    {
        // Don't do anything if the game day is done
        if(gameState.isDayDone()) {
            return;
        }

        // Check if the character has reached the last third of the screen on the x-axis
        if (!reachedPosition)
        {
            // Convert to world position, using the character’s current Y and Z to avoid overwriting those
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(targetXPosition, transform.position.y, Camera.main.nearClipPlane));
            
            // Move the character towards the target position
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

            // Bounce up and down around the initial Y position
            bounceTimer += Time.deltaTime;
            float newYPosition = initialYPosition + Mathf.Sin(bounceTimer * bounceFrequency) * bounceHeight;
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);

            if(transform.position.x <= targetXPosition) {
                reachedPosition = true;
            }
        } else {
            // Stop bouncing and set the y position to the fixed bottom third position
            transform.position = new Vector3(transform.position.x, targetYPosition, transform.position.z);
            dialogueBox.SetActive(true);
            customerMood.SetActive(true);

            // Decrease mood every 5 seconds by 1%
            moodTimer += Time.deltaTime;
            if (moodTimer >= 5f) {
                moodTimer = 0f;
                moodScore = Mathf.Max(0, moodScore - 1);
                SetCustomerMoodText(moodScore + "%");
            }
        }
    }

    public void NewCustomer() {
        // Reset position
        reachedPosition = false;
        transform.position = new Vector3(Screen.width, transform.position.y, transform.position.z);

        // Reset timers and scores
        bounceTimer = 0f;
        moodTimer = 0f;
        moodScore = 100;

        // Reset UI
        customerMood.SetActive(false);
        dialogueBox.SetActive(false);
        askButton.SetActive(true);

        // Select from 8 different badger images
        int randomIndex = Random.Range(0, 7);
        badgerImage.sprite = badgerSprites[randomIndex];

        // Reset order
        currentOrder = orderManager.generateCustomerOrder();

        // Reset dialogue and mood
        SetDialogueText(currentOrder.getCustomerOrder());
        SetCustomerMoodText(moodScore + "%");

        // Reset drink
        mixingDrink.reset();
    }

    public void EnterIngredientScene() {
        bartendingScene.SetActive(true);
        customerScene.SetActive(false);
    }

    public void EnterCustomerScene() {
        customerScene.SetActive(true);
        bartendingScene.SetActive(false);
    }

    public void RevealOrder() {
        SetDialogueText("I mean " + currentOrder.getDrinkName() + "!");
        int newMood = Mathf.Max(0, moodScore - 20);
        moodScore = newMood;
        SetCustomerMoodText(newMood + "%");
        askButton.SetActive(false);
    }

    public void SetDialogueText(string text) {
        dialogueText.text = text;
    }

    public void SetCustomerMoodText(string text) {
        customerMoodText.text = text;
    }

    public Drink getMixingDrink() {
        return this.mixingDrink;
    }
}