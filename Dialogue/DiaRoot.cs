using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DiaRoot : MonoBehaviour
{
    [SerializeField] private Transform StartingTrans = null;
    [SerializeField] private string NPC_name = "";

    public bool has_quest;
    public int dia_quest_objective_count;

    Dictionary<DiaNpcLine, bool> LineChecked;

    private void OnEnable()
    {
        check_for_quest();
    }

    public void check_for_quest()
    {
        if (!StartingTrans || !StartingTrans.GetComponent<DiaNpcLine>())
        {
            return;
        }
        LineChecked = new Dictionary<DiaNpcLine, bool>();
        has_quest = false;
        check_for_quest_helper(StartingTrans.GetComponent<DiaNpcLine>());
    }

    private void check_for_quest_helper(DiaNpcLine Line_in)
    {
        if (Line_in.HasQuest())
        {
            has_quest = true;
        }

        foreach(DiaPlayerLine DPL in Line_in.GetComponentsInChildren<DiaPlayerLine>())
        {
            if (!DPL.Check_Accessible(null))
            {
                continue;
            }

            if (DPL.HasQuest())
            {
                has_quest = true;
            }

            if (DPL.return_dest(false))
            {
                DiaNpcLine tempLine = DPL.return_dest(false).GetComponent<DiaNpcLine>();
                if (!LineChecked.ContainsKey(tempLine)) //This NPCLine has not been accessed before
                {
                    LineChecked.Add(tempLine, true);
                    check_for_quest_helper(tempLine);
                }
            }
        }

        if (Line_in.return_dest())
        {
            check_for_quest_helper(Line_in.return_dest().GetComponent<DiaNpcLine>());
        }
    }

    public void Modify_diaCount(bool add)
    {
        if (add)
        {
            dia_quest_objective_count += 1;
        }
        else
        {
            dia_quest_objective_count -= 1;
        }
    }


    public void ModifyStarting(Transform newStart)
    {
        StartingTrans = newStart;
        check_for_quest(); //Must be after
    }

    public Transform ReturnStarting()
    {
        return StartingTrans;
    }

    public string ReturnNPC_name()
    {
        return NPC_name;
    }

}
