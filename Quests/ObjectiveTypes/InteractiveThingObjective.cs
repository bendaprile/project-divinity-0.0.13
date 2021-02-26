using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InteractiveThingObjective : QuestObjective
{
    [SerializeField] private List<InteractiveThing> InteractiveThings = new List<InteractiveThing>();

    public override void initialize()
    {
        Assert.IsTrue(NumberOfTasks == InteractiveThings.Count);
        for (int i = 0; i < NumberOfTasks; ++i)
        {
            InteractiveThings[i].SetQuest(gameObject, i);
        }
        base.initialize();
    }
}
