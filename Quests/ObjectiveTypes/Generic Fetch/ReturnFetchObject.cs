using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ReturnFetchObject : QuestObjective
{
    [SerializeField] private GameObject ConnectedFetchQuest = null;
    [SerializeField] private GameObject ReturnDia = null;
    private Inventory inventory;

    private List<GameObject> itemRefs;

    public override void initialize()
    {
        Assert.IsTrue(NumberOfTasks == 1);
        base.initialize();
        itemRefs = ConnectedFetchQuest.GetComponent<FetchObjective>().ReturnItemRefs();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        ReturnDia.GetComponent<DiaMaster>().mark_dia_for_quest(gameObject, 0);
    }

    protected override void AdditionalObjectiveCompletionLogic()
    {
        foreach(GameObject item in itemRefs)
        {
            inventory.DeleteItem(item);
        }
    }
}
