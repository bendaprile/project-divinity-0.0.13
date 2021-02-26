using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

public class Sit : NPC_Activity
{
    [SerializeField] private Transform Object_Trans;
    [SerializeField] private Transform Sit_Loc;

    private Collider col;
    private NavMeshObstacle NMO;

    private void Start()
    {
        col = Object_Trans.GetComponent<Collider>();
        NMO = Object_Trans.GetComponent<NavMeshObstacle>();
        Assert.IsNotNull(col);
        Assert.IsNotNull(NMO);
        Assert.IsNotNull(Sit_Loc);
    }

    protected override void EnteredArea(Transform npc)
    {
        base.EnteredArea(npc);
        if (CurrentNPCs.Count == 1)
        {
            npc.GetComponent<NPC>().Walk(Sit_Loc.position);
        }
        else
        {
            CurrentNPCs[CurrentNPCs.Count - 1] = (CurrentNPCs[CurrentNPCs.Count - 1].Item1, Time.time, 0f); //Give new task
        }

    }

    protected override void LeftArea(Transform npc, int i)
    {
        npc.GetComponentInParent<HumanoidMaster>().ExternalAnimation(false);
    }

    protected override void CalledOnce()
    {
        col.enabled = CurrentNPCs.Count == 0;
        NMO.enabled = CurrentNPCs.Count == 0;
    }

    protected override float ActivityLogic(Transform npc, float misc_time, int i)
    {
        if (Vector2.Distance(new Vector2(npc.transform.position.x, npc.transform.position.z), new Vector2(Sit_Loc.position.x, Sit_Loc.position.z)) < 0.25f)
        {
            npc.GetComponentInParent<HumanoidMaster>().ExternalAnimation(anim: "sit");
            npc.parent.rotation = Sit_Loc.rotation;
        }

        return -1;
    }
}
