using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractiveObjectMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject ButtonPrefab = null;
    [SerializeField] private GameObject Name_ValuePrefab = null;
    [SerializeField] private GameObject Content;

    private Inventory inv;
    private GameObject PreviousContainer;

    private Transform ItemStatsTooltip;
    private Transform ItemPanel;
    private bool panelOpen;

    void Awake()
    {
        inv = GameObject.Find("Player").GetComponentInChildren<Inventory>();
        ItemStatsTooltip = transform.Find("ItemStatsTooltip");
        ItemPanel = transform.Find("ItemPanel");
        PreviousContainer = null;
        panelOpen = false;
    }

    public void Refresh(GameObject container)
    {
        PreviousContainer = container;
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }

        DisableStatPanel(true);

        List<GameObject> items = container.GetComponent<InteractiveBox>().ReturnItems(); //Reference
        
        
        for (int i = 0; i < items.Count; i++)
        { 
            GameObject temp = Instantiate(ButtonPrefab, Content.transform);
            temp.GetComponent<InteractiveUIPrefab>().Setup(items[i].GetComponent<ItemMaster>(), i, this);
        }

        if (!panelOpen)
        {
            Vector3 mousePostion = Input.mousePosition;
            ItemPanel.position = new Vector3(mousePostion.x + Screen.width * .1367f, mousePostion.y - 290f, ItemPanel.position.z);

            if (ItemPanel.localPosition.y < 350f)
            {
                ItemPanel.localPosition = new Vector3(ItemPanel.localPosition.x, 350f, ItemPanel.localPosition.z);
            }

            ItemPanel.GetComponent<Animator>().Play("In");
            ItemPanel.Find("Content").Find("ObjectName").GetComponent<TextMeshProUGUI>().text = container.GetComponent<InteractiveBox>().interactiveBoxName.ToUpper();
        }

        panelOpen = true;
    }

    public void TransferButtonPressed(int i)
    {
        if (inv.AddItem(PreviousContainer.GetComponent<InteractiveBox>().ReturnItem(i)))
        {
            Refresh(PreviousContainer);
            DisableStatPanel();
        }
    }

    public void EnableStatPanel(string itemName, string description, ItemQuality itemClass, List<(string, string)> data, Vector3 position)
    {
        DisableStatPanel();

        Vector3 rightContainer = ItemPanel.Find("RightContainer").position;
        if (rightContainer.x + 200f < Screen.width)
        {
            ItemStatsTooltip.position = new Vector3(rightContainer.x, position.y, position.z);
        }
        else
        {
            ItemStatsTooltip.position = new Vector3(ItemPanel.Find("LeftContainer").position.x, position.y, position.z);
        }

        if (ItemStatsTooltip.position.y / Screen.height < 0.25f)
        {
            ItemStatsTooltip.position = new Vector3(ItemStatsTooltip.position.x, Screen.height * 0.25f, ItemStatsTooltip.position.z);
        }

        ItemStatsTooltip.GetComponent<Animator>().Play("In");
        ItemStatsTooltip.Find("Content").Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemName.ToUpper();        ItemStatsTooltip.Find("Content").Find("Border").GetComponent<Image>().color = STARTUP_DECLARATIONS.itemQualityColors[itemClass];
        ItemStatsTooltip.Find("Content").Find("Description").GetComponent<TextMeshProUGUI>().text = description;

        foreach ((string, string) item in data)
        {
            GameObject temp = Instantiate(Name_ValuePrefab, ItemStatsTooltip.Find("Content").Find("Stats"));
            temp.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.Item1;
            temp.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = item.Item2;
        }
    }

    public void DisableStatPanel(bool startup = false)
    {
        foreach (Transform child in ItemStatsTooltip.Find("Content").Find("Stats"))
        {
            Destroy(child.gameObject);
        }
        if (!startup)
        {
            ItemStatsTooltip.gameObject.GetComponent<Animator>().Play("Out");
        }
    }

    public void DisablePanel()
    {
        DisableStatPanel(true);
        ItemPanel.GetComponent<Animator>().Play("Out");
        StartCoroutine(TurnOffPanel());
    }

    private IEnumerator TurnOffPanel()
    {
        panelOpen = false;
        yield return new WaitForSecondsRealtime(0.3f);

        if (!panelOpen) { gameObject.SetActive(false); }
    }

    private void OnDisable()
    {
        panelOpen = false;
    }
}