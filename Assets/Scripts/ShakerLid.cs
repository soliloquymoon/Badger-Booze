using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ShakerLid : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Canvas canvas;
    private bool finishMixing;

    void Start()
    {
        // Initialize references to RectTransform, parent Canvas, and the finishMixing flag
        rectTransform = this.GetComponent<RectTransform>();
        finishMixing = false;
        canvas = this.GetComponentInParent<Canvas>();
    }

    /*
     * OnBeginDrag: Called when dragging begins.
     * Saves the original position of the lid to return it to the initial position after dragging.
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = this.rectTransform.anchoredPosition;
    }

    /*
     * OnDrag: Called continuously while dragging.
     * Moves the lid based on the pointer (mouse or touch) movement during dragging.
     * The position is updated by adding the delta of the pointer's movement.
     */
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    /*
     * OnEndDrag: Called when dragging ends.
     * 1) If mixing is done, complete adding the ingredients and move to the shaking/pouring scene.
     * 2) If not, return the object to its original parent, sibling index, and position.
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        this.rectTransform.anchoredPosition = originalPosition;
        // if (finishMixing)
            // TODO: Move to the shaking/pouring scene
    }

    /*
     * OnTriggerEnter2D: Called when the ingredient object enters a 2D collision with another object.
     * If the other collider has the tag "Shaker Lid", it indicates that the shaker is closed.
     * Sets the finishMixing flag to true, indicating that mixing is complete.
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shaker Lid"))
        {
            Debug.Log("Shaker Closed!");
            finishMixing = true;
        }
    }

    /*
     * OnTriggerExit2D: Called when the ingredient object exits a 2D collision with another object.
     * If the other collider has the tag "Shaker Lid", it means the shaker lid is no longer closed.
     * Resets the finishMixing flag to false when the lid exits the collision zone.
     */
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shaker Lid"))
        {
            finishMixing = false;
        }
    }
}
