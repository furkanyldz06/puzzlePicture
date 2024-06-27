using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DummyInputManager : Singleton<DummyInputManager>
{

    public bool isActive;
    public List<RectTransform> activeImages=new List<RectTransform>();

    public RectTransform currentImage;
    public Camera mainCamera;
    public Canvas canvas;

    public float zoomSpeed;
    
    public Vector2 pivotPoint;





    public void SetCurrentImages(List<RectTransform> targetImages)
    {
        for (int i = 0; i < targetImages.Count; i++)
        {
            activeImages.Add(targetImages[i]);
        }
    }

    public void SetCurrentImage(GameObject go)
    {
        if (currentImage == null)
        {
            currentImage = go.GetComponent<RectTransform>();

            Vector2 localpoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(currentImage, Input.GetTouch(0).position, mainCamera, out localpoint);
            pivotPoint = Rect.PointToNormalized(currentImage.rect, localpoint);
            //currentImage.anchorMin = pivotPoint;
            //currentImage.anchorMax = pivotPoint;
            //currentImage.pivot = pivotPoint;
            //currentImage.SetSizeWithCurrentAnchors()
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(currentImage, Input.mousePosition, mainCamera, out pivotPoint);
            //pivotPoint = Rect.PointToNormalized(currentImage.rect, pivotPoint);

        }
    }

    // Update is called once per frame
    bool setPivot;
    void Update()
    {
        return;
        
        if (!isActive)
        {
            return;
        }
        if (Input.touchCount == 2 )//&& currentImage)
        {
            
            Zoom();

            //if (!setPivot)
            //{
            //    Vector2 localpoint;
            //    Vector2 midPoint = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2f;
            //    RectTransformUtility.ScreenPointToLocalPointInRectangle(currentImage, midPoint, mainCamera, out localpoint);
            //    pivotPoint = Rect.PointToNormalized(currentImage.rect, localpoint);
            //    setPivot = true;
            //}

            ////currentImage.pivot = Vector2.Lerp(currentImage.pivot, pivotPoint, Time.deltaTime*5f);


            //for (int i = 0; i < activeImages.Count; i++)
            //{
            //    activeImages[i].pivot= Vector2.Lerp(activeImages[i].pivot, pivotPoint, Time.deltaTime * 5f);
            //}
            ////currentImage.pivot = pivotPoint;

        }
        else
        {
            if (Input.GetMouseButton(0))// && currentImage)
            {
                //currentImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentImage.sizeDelta.x* scaleSpeed);
                //currentImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentImage.sizeDelta.y * scaleSpeed);
                //currentImage.pivot = Vector2.Lerp(currentImage.pivot, pivotPoint, Time.deltaTime);
                //currentImage.sizeDelta += Vector2.one* scaleSpeed;
                //currentImage.DOSizeDelta(Vector2.one,)
            }
            else
            {
                if (currentImage)
                {
                    currentImage = null;
                }
                setPivot = false;
            }
        }
    }


    public void PosSync(int index)
    {
        return;
        Vector3 pos = new Vector3();

        pos= activeImages[index].transform.localPosition;
        for (int i = 0; i < activeImages.Count; i++)
        {
            //if (i == 0)
            //{
            //    pos = activeImages[i].transform.localPosition;
            //}
            //else
            //{
            //    activeImages[i].transform.localPosition = pos;
            //}
            activeImages[i].transform.localPosition = pos;
        }
    }

    public void ResetZoom()
    {
        return;
        for (int i = 0; i < activeImages.Count; i++)
        {
            activeImages[i].transform.localScale = Vector3.one;
        }
    }

    private void Zoom()
    {
        return;
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // Find the position in the previous frame of each touch.
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Find the magnitude of the vector (the distance) between the touches in each frame.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Find the difference in the distances between each frame.
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        // ... change the canvas size based on the change in distance between the touches.


        
        for (int i = 0; i < activeImages.Count; i++)
        {
            Vector3 deltaVector = new Vector3((deltaMagnitudeDiff * zoomSpeed), (deltaMagnitudeDiff * zoomSpeed), 0);

            float x = Mathf.Clamp((activeImages[i].transform.localScale.x - deltaVector.x),1f,4.5f);
            float y = Mathf.Clamp((activeImages[i].transform.localScale.y - deltaVector.y), 1f, 4.5f);

            activeImages[i].transform.localScale = new Vector3(x, y, 0f);
            //activeImages[i].transform.localScale -= new Vector3((deltaMagnitudeDiff * zoomSpeed), 0f, 0f);
            //activeImages[i].transform.localScale -= new Vector3(0f,(deltaMagnitudeDiff * zoomSpeed), 0f);

            //activeImages[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, activeImages[i].sizeDelta.x - (deltaMagnitudeDiff * zoomSpeed));
            //activeImages[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, activeImages[i].sizeDelta.y - (deltaMagnitudeDiff * zoomSpeed));
        }
        //currentImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentImage.sizeDelta.x -(deltaMagnitudeDiff* zoomSpeed));
        //currentImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentImage.sizeDelta.y -(deltaMagnitudeDiff* zoomSpeed));

    }
}
