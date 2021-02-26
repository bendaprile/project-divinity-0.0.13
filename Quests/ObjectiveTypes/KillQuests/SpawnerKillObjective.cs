using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnerKillObjective : QuestObjective
{
    [SerializeField] private GameObject[] Spawners = new GameObject[1];
    [SerializeField] private bool Enable_or_SelfSpawn = false; //(COLLIDER NOT CODE) Must be true if the Spawner code is attached to the quest GameObject
    [SerializeField] private bool DisableSpawnerAfter = false;


    public override void initialize()
    {
        Assert.IsTrue(NumberOfTasks == Spawners.Length);
        base.initialize();

        for (int i = 0; i < NumberOfTasks; i++)
        {
            Spawners[i].GetComponent<Spawner>().AttachToQuest(Enable_or_SelfSpawn, gameObject, i);
        }
    }

    protected override void AdditionalTaskCompletionLogic(int num)
    {
        if (DisableSpawnerAfter)
        {
            Spawners[num].SetActive(false);
        }
    }

    public override List<(bool, string)> ReturnTasks()
    {
        List<(bool, string)> taskList = new List<(bool, string)>();

        for (int i = 0; i < NumberOfTasks; i++)
        {
            string cleared = TaskCompleted[i] ? "Cleared" : "";
            taskList.Add((TaskCompleted[i], (TaskDescription[i] + cleared)));
        }

        return taskList;
    }

    public override (bool, List<(Vector2, float)>) ReturnLocs()
    {
        return (true, ReturnLocsHelper());
    }
}
