using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FetchObjective : QuestObjective
{
    [SerializeField] private GameObject[] itemNeeded = new GameObject[1];
    [SerializeField] private int[] itemCountRequired = new int[1];

    [SerializeField] private bool LockItems = true;

    private List<ItemMaster> itemNeededSpecs = new List<ItemMaster>();
    private List<int> currentItems = new List<int>();
    private List<GameObject> itemRefs = new List<GameObject>();

    private Inventory inventory;

    public override void initialize()
    {
        Assert.AreEqual(NumberOfTasks, itemNeeded.Length);
        Assert.AreEqual(NumberOfTasks, itemCountRequired.Length);

        base.initialize();

        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        for (int i = 0; i < NumberOfTasks; i++)
        {
            currentItems.Add(0);
            ItemMaster ItemTypeNeeded = itemNeeded[i].GetComponent<ItemMaster>();
            itemNeededSpecs.Add(ItemTypeNeeded);

            List<GameObject> temp = inventory.ReturnItems_ByName(ItemTypeNeeded);

            foreach (GameObject iter in temp)
            {
                if (currentItems[i] < itemCountRequired[i])
                {
                    currentItems[i] += 1;
                    itemRefs.Add(iter);
                    if (LockItems)
                    {
                        inventory.LockItemQuest(iter);
                    }
                }
            }

            if(currentItems[i] == itemCountRequired[i])
            {
                TaskCompletion_Self(i);
            }
        }
    }

    public List<GameObject> ReturnItemRefs()
    {
        return itemRefs;
    }

    public void UpdateItemCount(GameObject item)
    {
        ItemMaster itemStats = item.GetComponent<ItemMaster>();
        for (int i = 0; i < NumberOfTasks; i++)
        {
            if (itemStats.ReturnBasicStats().Item3 == itemNeededSpecs[i].ReturnBasicStats().Item3 && currentItems[i] < itemCountRequired[i])
            {
                currentItems[i] += 1;
                itemRefs.Add(item);
                if (LockItems)
                {
                    inventory.LockItemQuest(item);
                }

                if (currentItems[i] == itemCountRequired[i])
                {
                    TaskCompletion_Self(i);
                }
            }
        }
    }

    public override List<(bool, string)> ReturnTasks()
    {
        List<(bool, string)> taskList = new List<(bool, string)>();

        for (int i = 0; i < NumberOfTasks; i++)
        {
            string amount_completed = " (" + currentItems[i] + "/" + itemCountRequired[i] + ")";
            taskList.Add((TaskCompleted[i], (TaskDescription[i] + amount_completed)));
        }

        return taskList;
    }

    public override ObjectiveType ReturnType()
    {
        return ObjectiveType.Fetch;
    }
}
