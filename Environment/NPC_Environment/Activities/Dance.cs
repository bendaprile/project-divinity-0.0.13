using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : NPC_Activity
{
    protected override void EnteredArea(Transform npc)
    {
        base.EnteredArea(npc);
        npc.GetComponentInParent<HumanoidMaster>().ExternalAnimation(anim: "dance");
    }

    protected override void LeftArea(Transform npc, int i)
    {
        npc.GetComponentInParent<HumanoidMaster>().ExternalAnimation(false);
    }
}
