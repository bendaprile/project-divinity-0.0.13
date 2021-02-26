using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRotatingDoor : InteractiveThing
{
    [SerializeField] protected Material[] materials = null;
    [SerializeField] int LockPickingRequirement = 0;
    private PlayerStats playerStats;
    private MeshRenderer mesh;
    private MiscDisplay miscDisplay;
    private DoorController doorController;

    InteractiveRotatingDoor()
    {
        MiscDisplayUsed = true;
    }


    protected override void Awake()
    {
        base.Awake();
        mesh = GetComponent<MeshRenderer>();
        playerStats = player.GetComponent<PlayerStats>();
        miscDisplay = FindObjectOfType<MiscDisplay>();
        doorController = GetComponent<DoorController>();
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
        string openCloseText = doorController.open ? "Close" : "Open";
        if (LockPickingRequirement == 0)
        {
            miscDisplay.enableDisplay("", "(E) " + openCloseText);
            if (Input.GetKeyDown(KeyCode.E))
            {
                QuestActiveLogic();
                doorController.ActivateDoor();
            }
        }
        else if (playerStats.ReturnNonCombatSkill(SkillsEnum.Larceny) >= LockPickingRequirement)
        {
            miscDisplay.enableDisplay("", "Larceny [" + playerStats.ReturnNonCombatSkill(SkillsEnum.Larceny) + "/" + LockPickingRequirement + "] (E)");
            if (Input.GetKeyDown(KeyCode.E))
            {
                QuestActiveLogic();
                doorController.ActivateDoor();
            }
        }
        else
        {
            miscDisplay.enableDisplay("", "Larceny [" + playerStats.ReturnNonCombatSkill(SkillsEnum.Larceny) + "/" + LockPickingRequirement + "]");
        }
    }
}
