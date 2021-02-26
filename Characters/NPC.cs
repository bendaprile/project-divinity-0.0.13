using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NPC : MonoBehaviour
{
    [SerializeField] private Transform SleepingLocation;
    [SerializeField] private List<Transform> CustomActivities; 
    [SerializeField] private List<NPC_FactionsEnum> NPC_Facs;
    [SerializeField] private List<int> NPC_Facs_Percentages_input;

    private List<int> NPC_Facs_Percentages = new List<int>(); //This holdes the upperbound e.g. 25 30 30 15 => 25,55,85,100

    private HumanoidMaster HM;
    private NPC_Logic NL;
    private DayNightController DNC;

    private NPCActivityFlag ActivityFlag = NPCActivityFlag._NO_FLAG_; //Used for activity based Dia

    private void Awake()
    {
        int total = 0;
        for(int i = 0; i < NPC_Facs_Percentages_input.Count; ++i)
        {
            total += NPC_Facs_Percentages_input[i];
            NPC_Facs_Percentages.Add(total);
        }

        if(NPC_Facs.Count != 0)
        {
            Assert.IsTrue(total == 100);
            Assert.IsTrue(CustomActivities.Count == 0);
        }
        Assert.IsTrue(NPC_Facs.Count == NPC_Facs_Percentages_input.Count);


        HM = GetComponentInParent<HumanoidMaster>();
        NL = FindObjectOfType<NPC_Logic>();
        DNC = FindObjectOfType<DayNightController>();
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f);
        RandomTask();
    }

    public void Set_ActivityFlag(NPCActivityFlag set)
    {
        ActivityFlag = set;
    }

    public string ReturnFactionDiaLine()
    {
        return NL.ReturnFactionLine(ActivityFlag, NPC_Facs, NPC_Facs_Percentages);
    }

    public void RandomTask()
    {
        if (HM.Return_Control_Mode() != NPC_Control_Mode.NPC_control)
        {
            return;
        }

        Transform activity;
        if (SleepingLocation && DNC.isNight)
        {
            activity = SleepingLocation;
        }
        else if(CustomActivities.Count > 0)
        {
            activity = CustomActivities[Random.Range(0, CustomActivities.Count)];
        }
        else
        {
            activity = NL.FindDest(NPC_Facs, NPC_Facs_Percentages);
        }

        NPC_Activity npcAct = activity.GetComponentInChildren<NPC_Activity>();

        if (npcAct.ReturnInUse())
        {
            StartCoroutine(RandomTask_NextFrame());
            return;
        }

        Walk(npcAct.transform.position);
        activity.GetComponentInChildren<NPC_Activity>().AddMigratingNPC(transform);
    }

    IEnumerator RandomTask_NextFrame()
    {
        yield return new WaitForEndOfFrame();
        RandomTask();
    }


    public void Walk(Vector3 Location)
    {
        if (HM.Return_Control_Mode() != NPC_Control_Mode.NPC_control)
        {
            return;
        }

        HM.MoveToDest(Location);
    }
}
