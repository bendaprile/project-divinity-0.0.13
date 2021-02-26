using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImplantUIHolder : MonoBehaviour
{
    [SerializeField] GameObject ImplantUIPrefab = null;
    private Inventory InventoryScript = null;
    private Transform tempStorage = null;

    private bool firstStart = true;

    void FirstStart()
    {
        InventoryScript = GameObject.Find("Player").GetComponentInChildren<Inventory>();
        tempStorage = GameObject.Find("ImplantTempRemoveStorage").transform;
        firstStart = false;
    }

    private void OnEnable()
    {
        if (firstStart)
        {
            FirstStart();
        }
        Refresh();
    }

    public void Refresh()
    {
        foreach(Transform child in transform) //Destroys after frame
        {
            Destroy(child.gameObject);
        }

        List<GameObject> items = InventoryScript.ReturnItems(ItemTypeEnum.Implant);

        foreach (GameObject item in items)
        {
            GameObject UIFab = Instantiate(ImplantUIPrefab, transform);
            UIFab.GetComponent<ImplantUIPrefab>().Setup(true, item, GetComponent<ImplantUIHolder>(), InventoryScript);
        }

        foreach (Transform item in tempStorage)
        {
            GameObject UIFab = Instantiate(ImplantUIPrefab, transform);
            UIFab.GetComponent<ImplantUIPrefab>().Setup(false, item.gameObject, GetComponent<ImplantUIHolder>(), InventoryScript);
        }
    }
}
