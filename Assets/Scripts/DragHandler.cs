using UnityEngine;
using UnityEngine.EventSystems;


public class DragHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
   private RectTransform rectTransform;
   private Vector2 originalPosition;
   private Quaternion originalRotation; // Store the initial rotation of the lid


   [SerializeField] private GameObject shaker; // Reference to the Shaker GameObject
   [SerializeField] private float snapDistance = 50f;
   private Vector2 shakerPosition;


   void Start()
   {
       rectTransform = GetComponent<RectTransform>();
       originalPosition = rectTransform.anchoredPosition;
       originalRotation = rectTransform.rotation; // Store the initial rotation


       // Get the shaker's anchored position from its RectTransform
       if (shaker != null)
       {
           RectTransform shakerRect = shaker.GetComponent<RectTransform>();
           shakerPosition = shakerRect.anchoredPosition;
       }
       else
       {
           Debug.LogError("Shaker GameObject is not assigned in the Inspector.");
       }
   }


   public void OnPointerDown(PointerEventData eventData) { }


   public void OnDrag(PointerEventData eventData)
   {
       rectTransform.anchoredPosition += eventData.delta;
   }


   public void OnPointerUp(PointerEventData eventData)
   {
       if (Vector2.Distance(rectTransform.anchoredPosition, shakerPosition) < snapDistance)
       {
           rectTransform.anchoredPosition = shakerPosition;
           rectTransform.rotation = Quaternion.identity;
       }
       else
       {
           rectTransform.anchoredPosition = originalPosition;
           rectTransform.rotation = originalRotation; // Restore the original rotation
       }
   }
}
