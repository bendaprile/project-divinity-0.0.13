using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DestinationObjective : QuestObjective //NOT TESTED
{
    private Collider colliderOnObject; //START DISABlED //Must be on Quest Object

    public override void initialize()
    {
        colliderOnObject = GetComponent<Collider>();
        Assert.IsFalse(colliderOnObject.enabled); //This must start disabled
        Assert.IsTrue(NumberOfTasks == 1);
        colliderOnObject.enabled = true;

        base.initialize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            colliderOnObject.enabled = false;
            TaskCompletion_Self(0);
        }
    }
}
