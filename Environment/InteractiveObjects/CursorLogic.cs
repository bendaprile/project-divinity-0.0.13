using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLogic : MonoBehaviour
{
    private Transform playerTrans;
    private GameObject currentInteractiveThing, lastInteractiveThing;
    private Vector3 mousePos3d = new Vector3();
    private Vector3 Player2Cursor = new Vector3();

    void Start()
    {
        playerTrans = GameObject.Find("Player").GetComponent<Transform>();
        currentInteractiveThing = null;
        lastInteractiveThing = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InteractiveObjectFunction();
        GetPosition();
    }

    public Vector3 ReturnMousePos()
    {
        return mousePos3d;
    }

    public Vector3 ReturnPlayer2Cursor()
    {
        return Player2Cursor;
    }

    // TODO: Handle for controllers
    private void InteractiveObjectFunction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = (LayerMask.GetMask("InteractiveThing"));
        RaycastHit hitray;

        lastInteractiveThing = currentInteractiveThing;
        if (Physics.Raycast(ray, out hitray, Mathf.Infinity, layerMask))
        {
            currentInteractiveThing = hitray.transform.gameObject;
        }
        else
        {
            currentInteractiveThing = null;
        }


        if (currentInteractiveThing != lastInteractiveThing)
        {
            if (currentInteractiveThing != null)
            {
                currentInteractiveThing.GetComponentInChildren<InteractiveThing>().CursorOverObject();
            }

            if (lastInteractiveThing != null)
            {
                lastInteractiveThing.GetComponentInChildren<InteractiveThing>().CursorLeftObject();
            }
        }
    }

    // TODO: Handle for controllers
    private void GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = (LayerMask.GetMask("Terrain"));
        RaycastHit hitray;

        if (Physics.Raycast(ray, out hitray, Mathf.Infinity, layerMask))
        {
            mousePos3d = hitray.point;
        }

        Player2Cursor = mousePos3d - playerTrans.position;
    }
}
