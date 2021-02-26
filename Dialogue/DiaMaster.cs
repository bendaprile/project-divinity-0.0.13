using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaMaster : MonoBehaviour
{
    [SerializeField] protected Transform Quest = null; //null if no quest (This is a quest to give)

    public GameObject QuestTaskReturn; //null if no quest,
    public int quest_num;


    public bool HasQuest()
    {
        return Quest;
    }

    public void mark_dia_for_quest(GameObject quest_in, int quest_num_in)
    {
        GetComponentInParent<DiaRoot>().Modify_diaCount(true);
        QuestTaskReturn = quest_in;
        quest_num = quest_num_in;
    }
}
