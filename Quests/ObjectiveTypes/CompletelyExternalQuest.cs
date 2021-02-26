using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletelyExternalQuest : QuestObjective //TODO REMOVE THIS WHOLE SCRIPT
{
    void Start()
    {
        //Assert.AreNotEqual(ReturnLocation, null);
        base.initialize();
    }

    public override (bool, List<(Vector2, float)>) ReturnLocs()
    {
        List<(Vector2, float)> tempList = new List<(Vector2, float)>();
        return (false, tempList);
    }
}
