using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Activity : MonoBehaviour
{
    [SerializeField] protected bool Set_Duration = true;
    [SerializeField] float Activity_duration = 10f;
    [SerializeField] NPCActivityFlag npcAF = NPCActivityFlag._NO_FLAG_;

    protected List<Transform> MigratingNPCs = new List<Transform>();
    protected List<(Transform, float, float)> CurrentNPCs = new List<(Transform, float, float)>(); //NPC, exit time, misc time

    public virtual bool ReturnInUse() //Change if the task supports multiple npcs
    {
        return MigratingNPCs.Count > 0 || CurrentNPCs.Count > 0;
    }

    public void AddMigratingNPC(Transform npc)
    {
        MigratingNPCs.Add(npc);
    }

    private void OnTriggerStay(Collider other)
    {
        for(int i = 0; i < MigratingNPCs.Count; ++i)
        {
            if(other.transform == MigratingNPCs[i])
            {
                CurrentNPCs.Add((MigratingNPCs[i], Time.time + Activity_duration, Time.time));
                MigratingNPCs.RemoveAt(i);
                EnteredArea(CurrentNPCs[CurrentNPCs.Count -1].Item1);
                break;
            }
        }
    }



    protected void NPC_finished(Transform npc, int i) 
    {
        npc.GetComponent<NPC>().Set_ActivityFlag(NPCActivityFlag._NO_FLAG_);
        npc.GetComponent<NPC>().RandomTask();
        LeftArea(npc, i);
        CurrentNPCs.RemoveAt(i);
    } 

    protected virtual void EnteredArea(Transform npc)
    {
        npc.GetComponent<NPC>().Set_ActivityFlag(npcAF);
    }

    protected virtual void LeftArea(Transform npc, int i)
    {
    }

    protected virtual float ActivityLogic(Transform npc, float misc_time, int i)
    {
        return -1;
    }

    protected virtual void CalledOnce()
    {

    }

    private void FixedUpdate()
    {
        CalledOnce();

        for (int i = 0; i < CurrentNPCs.Count; ++i)
        {
            bool condition1 = Set_Duration && CurrentNPCs[i].Item2 <= Time.time;
            bool condition2;
            if (CurrentNPCs[i].Item1)
            {
                condition2 = CurrentNPCs[i].Item1.GetComponentInParent<HumanoidMaster>().Return_Control_Mode() != NPC_Control_Mode.NPC_control;
            }
            else
            {
                condition2 = false;
            }

            if (condition1 | condition2)
            {
                NPC_finished(CurrentNPCs[i].Item1, i);
            }
            else
            {
                float newMisc = ActivityLogic(CurrentNPCs[i].Item1, CurrentNPCs[i].Item3, i);
                if (newMisc != -1)
                {
                    CurrentNPCs[i] = (CurrentNPCs[i].Item1, CurrentNPCs[i].Item2, newMisc);
                }
            }
        }
    }
}
