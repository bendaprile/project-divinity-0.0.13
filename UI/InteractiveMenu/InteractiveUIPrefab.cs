using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InteractiveUIPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InteractiveObjectMenuUI IOM;
    private ItemMaster ITEM;
    private int itemStorageLoc;

    private bool stats_enabled = false;
    private bool cursor_over = false;
    private float cursor_over_time = 0f;

    public void Setup(ItemMaster itemProperties, int storage_loc, InteractiveObjectMenuUI IOM)
    {
        this.IOM = IOM;
        ITEM = itemProperties;
        transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemProperties.ReturnBasicStats().Item3.ToUpper();
        transform.Find("Preview").Find("Icon").GetComponent<Image>().sprite = itemProperties.ReturnBasicStats().Item4;
        transform.Find("QualityIndicator").GetComponent<Image>().color = GetQualityColor(itemProperties.ReturnBasicStats().Item5);
        transform.Find("Weight").Find("WeightText").GetComponent<TextMeshProUGUI>().text = itemProperties.ReturnBasicStats().Item2.ToString();
        transform.Find("Cost").Find("CostText").GetComponent<TextMeshProUGUI>().text = itemProperties.ReturnBasicStats().Item1.ToString();
        transform.Find("Description").GetComponent<TextMeshProUGUI>().text = GetItemType(itemProperties);
        itemStorageLoc = storage_loc;
    }

    private Color32 GetQualityColor(ItemQuality itemClass)
    {
        return STARTUP_DECLARATIONS.itemQualityColors[itemClass];
    }

    private string GetItemType(ItemMaster item)
    {
        switch (item.ReturnItemType())
        {
            case ItemTypeEnum.Weapon:
                return item.GetComponent<Weapon>().ReturnWeaponType().ToString() + " Weapon";
            case ItemTypeEnum.Armor:
                return item.GetComponent<Armor>().returnArmorType().ToString() + " Armor";
            case ItemTypeEnum.Consumable:
                return "Consumable";
            case ItemTypeEnum.Misc:
                return "Misc";
            case ItemTypeEnum.Implant:
                return "Implant";
            default:
                Debug.LogError("Item Type Not Recognized!");
                return "";
        }
    }

    public void ButtonPressedTransfer()
    {
        IOM.TransferButtonPressed(itemStorageLoc);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursor_over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursor_over = false;
        stats_enabled = false;
        IOM.DisableStatPanel();
        cursor_over_time = 0;
    }

    void Update()
    {
        if (cursor_over)
        {
            cursor_over_time += Time.unscaledDeltaTime;
        }

        if (cursor_over_time >= STARTUP_DECLARATIONS.TIME_TO_DISPLAY_TOOLTIP)
        {
            if (!stats_enabled)
            {
                stats_enabled = true;
                IOM.EnableStatPanel(ITEM.ReturnBasicStats().Item3, ITEM.ReturnAdvStats().Item2, ITEM.ReturnBasicStats().Item5, ITEM.ReturnAdvStats().Item1, transform.position);
            }
        }
    }
}