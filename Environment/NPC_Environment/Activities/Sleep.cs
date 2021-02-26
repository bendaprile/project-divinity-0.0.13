using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Sleep : NPC_Activity
{
    private DayNightController DNC;

    private void Start()
    {
        Assert.IsFalse(Set_Duration);
        DNC = FindObjectOfType<DayNightController>();
    }

    protected override float ActivityLogic(Transform npc, float misc_time, int i)
    {
        if (!DNC.isNight)
        {
            NPC_finished(npc, i);
        }
        return -1;
    }
}
