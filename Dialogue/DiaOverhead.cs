using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DiaOverhead : DiaMaster
{
    [SerializeField] [TextArea(3, 10)] private List<string> PersonalLines = new List<string>();
    [SerializeField] int PersonalLinesPercentage = 0; // 0 to 100
    [SerializeField] private bool wrapAround = false;

    private NPC npc;
    private int iter = 0;

    private void Awake()
    {
        Assert.IsNull(Quest);
        npc = GetComponentInParent<HumanoidMaster>().npc;
    }

    public string return_line()
    {
        int RandNum = Random.Range(0, 100);

        if (RandNum < PersonalLinesPercentage)
        {
            return return_personal_line();
        }
        else
        {
            return npc.ReturnFactionDiaLine();
        }
    }

    private string return_personal_line()
    {
        int tempIter = iter;
        iter += 1;
        if (iter == PersonalLines.Count)
        {
            if (wrapAround)
            {
                iter = 0;
            }
            else
            {
                iter = tempIter;
            }
        }
        return PersonalLines[tempIter];
    }
}
