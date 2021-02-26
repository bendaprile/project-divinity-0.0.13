using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImplantPrefab : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Inventory inv;
    private ImplantUIHolder iUIh;
    private Transform TempStorage;

    private Canvas canvas;
    private RectTransform rectTransform;

    private int startingX; //left
    private int startingY; //bottom 
    private bool[,] used_locs; //from bottom left
    public int sizeX;
    public int sizeY;

    private bool cursor_over = false;
    private bool selected = false;

    private bool first_start = true;

    //(0,2) (1,2) (2,2)
    //(0,1) (1,1) (2,1)
    //(0,0) (1,0) (2,0)

    private void First_Start()
    {
        if (first_start)
        {
            first_start = false;
            GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, 0f);
            GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            TempStorage = GameObject.Find("ImplantTempRemoveStorage").transform;
            iUIh = GameObject.Find("ImplantHolder").GetComponent<ImplantUIHolder>();
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            canvas = GetComponentInParent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            SetupArray();
        }
    }

    private void SetupArray()
    {
        sizeX = (int)rectTransform.sizeDelta.x / 50;
        sizeY = (int)rectTransform.sizeDelta.y / 50;

        used_locs = new bool[sizeX, sizeY];
        foreach (Transform child in transform.Find("Images"))
        {
            RectTransform tempRect = child.GetComponent<RectTransform>();
            int StartX = (int)tempRect.anchoredPosition.x / 50;
            int EndX = (int)(tempRect.anchoredPosition.x + tempRect.sizeDelta.x) / 50;
            int StartY = (int)tempRect.anchoredPosition.y / 50;
            int EndY = (int)(tempRect.anchoredPosition.y + tempRect.sizeDelta.y) / 50;

            for (int i = StartX; i < EndX; i++)
            {
                for (int j = StartY; j < EndY; j++)
                {
                    used_locs[i, j] = true;
                }
            }
        }
    }

    public void Setup()
    {
        gameObject.SetActive(true);
        First_Start();
        SnapToCell();
    }

    private void Update()
    {
        if(cursor_over & Input.GetMouseButton(0))
        {
            selected = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            selected = false;
        }

        if (Input.GetKeyDown("r") && selected)
        {
            rotation();
        }
    }

    public bool ReturnCellUsed(int i, int j)
    {
        int xLoc = i - startingX;
        int yLoc = j - startingY;

        if (xLoc >= 0 && xLoc < sizeX && yLoc >= 0 && yLoc < sizeY)
        {
            return used_locs[xLoc, yLoc];
        }
        else
        {
            return false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToCell();
        CheckOutbounds();
    }

    private void SnapToCell()
    {
        float x_offset = (rectTransform.sizeDelta.x % 100) / 2;
        float y_offset = (rectTransform.sizeDelta.y % 100) / 2;

        rectTransform.anchoredPosition = new Vector2(FindNearestSnap(rectTransform.anchoredPosition.x, x_offset), FindNearestSnap(rectTransform.anchoredPosition.y, y_offset));
        UpdateStartingLocs();
    }

    private void CheckOutbounds()
    {
        int maxX = 16 - sizeX;
        int maxY = 16 - sizeY;

        if (startingX < 0 || startingX > maxX || startingY < 0 || startingY > maxY)
        {
            transform.SetParent(TempStorage);
            gameObject.SetActive(false);
            iUIh.Refresh();
        }
    }

    private float FindNearestSnap(float xy, float offset)
    {
        float mod = (xy - offset) % 50;
        if (mod > 25)
        {
            return (xy - mod + 50);
        }
        else
        {
            return (xy - mod);
        }
    }

    private void UpdateStartingLocs()
    {
        startingX = (int)(rectTransform.anchoredPosition.x - (rectTransform.sizeDelta.x / 2)) / 50;
        startingY = (int)(rectTransform.anchoredPosition.y - (rectTransform.sizeDelta.y / 2)) / 50;
    }

    private void rotation()
    {
        foreach (Transform child in transform.Find("Images"))
        {
            RectTransform tempRect = child.GetComponent<RectTransform>();

            tempRect.anchoredPosition = new Vector3(-tempRect.anchoredPosition.y - tempRect.sizeDelta.y + rectTransform.sizeDelta.y, tempRect.anchoredPosition.x, 0);
            tempRect.sizeDelta = new Vector3(tempRect.sizeDelta.y, tempRect.sizeDelta.x, 0);
        }
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y, rectTransform.sizeDelta.x);
        SetupArray();
        UpdateStartingLocs();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursor_over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursor_over = false;
    }
}
