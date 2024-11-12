using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private int originalSiblingIndex;
    private Animator animator;


    void Start()
    {
        // Initialize references for RectTransform, parent, canvas, and animator
        rectTransform = this.GetComponent<RectTransform>();
        originalParent = this.transform.parent;
        canvas = this.GetComponentInParent<Canvas>();
        animator = this.GetComponent<Animator>();
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
     * If the other object has the "Shaker" tag and is currently a child of the canvas,
     * start the "Ingredient_Pour" animation.
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shaker") && this.rectTransform.parent == canvas.transform)
        {
            animator.SetBool("Pour", true);
        }
    }

    /*
     * OnTriggerExit2D: Called when the ingredient object exits a 2D collision with another object.
     * If the other object has the "Shaker" tag, start the "Ingredient_Return" animation.
     */
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shaker"))
        {
            animator.SetBool("Pour", false);
        }
    }
}
