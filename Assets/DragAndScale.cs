using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragAndScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{ 
    RectTransform rectTransform; // Assign this through the inspector
    [SerializeField] RectTransform rectTransform2;
    private float scaleFactor = 1.75f;
    private Vector3 originalScale;
    private Vector2 originalPosition;
    private bool isDragging = false;
    private Vector2 clickOffset;

    void Start()
    {
        // Store the original scale and position
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("SSSSSDSADS");
        
        if(PlayerPrefs.GetInt("Zoom") == 0) 
            return;
        
        StartCoroutine(interactable(0,false));

        // Calculate the offset between the mouse position and the rectTransform's position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out clickOffset);
        
        // Calculate the pivot based on the click position relative to the rectTransform's size
        Vector2 pivot = new Vector2(
            (eventData.position.x - rectTransform.position.x) / rectTransform.rect.width,
            (eventData.position.y - rectTransform.position.y) / rectTransform.rect.height
        );
        
        // Adjust the pivot position in local coordinates
        Vector2 pivotPosition = new Vector2(
            pivot.x * rectTransform.rect.width,
            pivot.y * rectTransform.rect.height
        );
        
        // Adjust the anchored position based on the pivot position
        rectTransform.anchoredPosition = eventData.position - pivotPosition;
        
        rectTransform2.anchoredPosition = eventData.position - pivotPosition;

        // Increase the scale
        rectTransform.localScale = originalScale * scaleFactor;
        rectTransform2.localScale = originalScale * scaleFactor;

        
        isDragging = true;
        


        // RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out clickOffset);
        //
        // // Adjust the pivot to the click position
        // Vector2 pivot = new Vector2(clickOffset.x / rectTransform.rect.width, clickOffset.y / rectTransform.rect.height);
        // rectTransform.pivot = pivot;
        //
        // // Adjust the anchored position to keep the same position visually
        // rectTransform.anchoredPosition -= new Vector2(rectTransform.rect.width * rectTransform.pivot.x, rectTransform.rect.height * rectTransform.pivot.y);
        //
        // // Increase the scale
        // rectTransform.localScale = originalScale * scaleFactor;
        //
        // isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // Move the RectTransform with the mouse
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
            rectTransform.anchoredPosition = localPoint - clickOffset;
            rectTransform2.anchoredPosition = localPoint - clickOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        imageBack();
    }

    public void imageBack()
    {
        if(PlayerPrefs.GetInt("Zoom") == 0)
            return;
        
        rectTransform.localScale = originalScale;
        rectTransform.anchoredPosition = originalPosition;
        
        rectTransform2.localScale = originalScale;
        rectTransform2.anchoredPosition = originalPosition;
        
        StartCoroutine(interactable(0.01f,true));

        isDragging = false;
    }

    public IEnumerator interactable(float sec, bool val)
    {
        yield return new WaitForSeconds(sec);
        
        GetComponent<UnityEngine.UI.Button>().interactable = val;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().enabled = val;
        }
        if(val == true)
            GameManager.instance.getZoomOut();
    }
}
