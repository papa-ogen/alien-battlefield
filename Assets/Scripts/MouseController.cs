using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D lightCoverCursorTexture;
    public Texture2D hardCoverCursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit raycastHit;

        if(Physics.Raycast(ray, out raycastHit))
        {
            GameObject hitObject = raycastHit.transform.root.gameObject;

            ChangeCursor(hitObject);
        }
    }

    void ChangeCursor(GameObject obj)
    {
        Cover cover = obj.GetComponent<Cover>();
        Debug.Log("cover" + obj.name);
        /*
        if(cover != null)
        {
            if(cover.CoverType == CoverType.LightCover)
            {
                Cursor.SetCursor(lightCoverCursorTexture, hotSpot, cursorMode);
                return;

            } else if(cover.CoverType == CoverType.HardCover)
            {
                Cursor.SetCursor(hardCoverCursorTexture, hotSpot, cursorMode);
                return;
            }
        }
        */
       // Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}
