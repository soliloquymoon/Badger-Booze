using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Customer customer;
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private int originalSiblingIndex;
    private Animator animator;
    private Image image;
    private Coroutine pouring;
    private GameState gameState;

    void Start()
    {
        // Initialize references for RectTransform, parent, canvas, animator, and image
        rectTransform = this.GetComponent<RectTransform>();
        originalParent = this.transform.parent;
        canvas = this.GetComponentInParent<Canvas>();
        animator = this.GetComponent<Animator>();
        image = this.GetComponent<Image>();

        // Find customer, GameState, and receipt objects
        customer = GameObject.FindGameObjectWithTag("Customer").GetComponent<Customer>();
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    /*
     * OnBeginDrag: Called when dragging begins.
     * 1) Saves the original sibling index and position to allow resetting after dragging.
     * 2) Changes the parent to the canvas to allow movement outside of the scroll bar bounds.
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalSiblingIndex = this.transform.GetSiblingIndex();
        originalPosition = this.rectTransform.anchoredPosition;
        transform.SetParent(canvas.transform, true);
    }

    /*
     * OnDrag: Called continuously while dragging.
     * Moves the object based on mouse delta position, adjusted by canvas scaling factor.
     */
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    /*
     * OnEndDrag: Called when dragging ends.
     * 1) Returns the object to its original parent, sibling index, and position.
     * 2) Resets the animator state to its default.
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(originalParent, true);
        this.rectTransform.anchoredPosition = originalPosition;
        this.transform.SetSiblingIndex(originalSiblingIndex);
        this.animator.SetTrigger("Default");
        this.animator.SetBool("Pour", false);
    }

    /*
     * OnTriggerEnter2D: Called when the ingredient object enters a 2D collision with another object.
     * If the other object has the "Shaker" tag and is currently a child of the canvas, this method
     * 1) starts the "Ingredient_Pour" animation
     * 2) and start a coroutine to add the ingredient.
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shaker") && this.rectTransform.parent == canvas.transform)
        {
            animator.SetBool("Pour", true);
            pouring = StartCoroutine(AddIngredient(customer.GetMixingDrink()));
        }
    }

    /*
     * OnTriggerExit2D: Called when the ingredient object exits a 2D collision with another object.
     * If the other object has the "Shaker" tag, this method
     * 1) switches the animation from "Ingredient_Pour" to "Ingredient_Return"
     * 3) cancels the ingredient-adding coroutine
     */
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shaker"))
        {
            animator.SetBool("Pour", false);
            if (pouring != null) {
                StopCoroutine(pouring);
                pouring = null;
            }
        }
    }

    /*
    * AddIngredient: Coroutine that continuously adds an ingredient to the Drink object.
    * 
    * This method is executed as a coroutine, which allows for a delay between each addition.
    * It runs indefinitely and repeatedly calls `addIngredient()` on the `drink` object,
    * passing the name of the current object (=ingredient) each time.
    * 
    * It has a very short delay (0.001f), causing the ingredient to be added at a very high frequency.
    */
    IEnumerator AddIngredient(Drink drink) {
        while (true) {
            yield return new WaitForSecondsRealtime(0.0002f);
            // Pour drink multiple times to make it seem faster
            for(int i = 0; i < 8; i++) {
                drink.AddIngredient(this.name);
                gameState.DeductMoney(0.0002f);
            }
            customer.GetReceipt().SetText(drink, this.name);
        }
    }

    public void SetName(string name) {
        this.name = name;
    }

    public void SetSprite(Sprite sprite) {
        image = this.GetComponent<Image>();

        this.image.sprite = sprite;
        this.image.SetNativeSize();
    }
}
