using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveBox : InteractiveThing
{
    public string interactiveBoxName = "Chest";
    [SerializeField] protected Material[] materials = null;
    [SerializeField] int LockPickingRequirement = 0;

    private Transform ItemHolder;
    private PlayerStats playerStats;
    private MeshRenderer mesh;

    public GameObject ReturnItem(int i)
    {
        return ItemHolder.GetChild(i).gameObject;
    }

    public List<GameObject> ReturnItems()
    {
        List<GameObject> tempItems = new List<GameObject>();
        foreach(Transform item in ItemHolder)
        {
            tempItems.Add(item.gameObject);
        }
        return tempItems;
    }


    protected override void Awake()
    {
        base.Awake();
        mesh = GetComponent<MeshRenderer>();
        UIControl = GameObject.Find("UI").GetComponent<UIController>();
        playerStats = player.GetComponent<PlayerStats>();
        ItemHolder = transform.Find("ItemHolder");
    }

    public override void CursorOverObject()
    {
        base.CursorOverObject();
        set_item_material(1);
    }

    public override void CursorLeftObject()
    {
        base.CursorLeftObject();
        set_item_material(0);
    }

    protected virtual void set_item_material(int i)
    {
        mesh.material = materials[i];
    }

    // TODO: Handle Input through InputManager and not direct key references
    protected override void ActivateLogic()
    {
        if(LockPickingRequirement == 0)
        {
            Text.GetComponent<TextMeshPro>().text = "(E) Open";
            if (Input.GetKeyDown(KeyCode.E))
            {
                QuestActiveLogic();
                UIControl.OpenInteractiveMenu(gameObject);
            }
        }
        else if (playerStats.ReturnNonCombatSkill(SkillsEnum.Larceny) >= LockPickingRequirement)
        {
            Text.GetComponent<TextMeshPro>().text = "Larceny [" + playerStats.ReturnNonCombatSkill(SkillsEnum.Larceny) + "/" + LockPickingRequirement + "] (E)";
            if (Input.GetKeyDown(KeyCode.E))
            {
                QuestActiveLogic();
                UIControl.OpenInteractiveMenu(gameObject);
            }
        }
        else
        {
            Text.GetComponent<TextMeshPro>().text = ("Larceny [" + playerStats.ReturnNonCombatSkill(SkillsEnum.Larceny) + "/" + LockPickingRequirement + "]");
        }
    }
}
