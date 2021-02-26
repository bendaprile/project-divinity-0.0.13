using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GenericTalkObjective : QuestObjective
{
    [SerializeField] private GameObject[] ExactDiaLine = new GameObject[1];

    [SerializeField] List<DiaRoot> DiaRoots;
    [SerializeField] List<Transform> NewStarts;

    private void Awake()
    {
        Assert.IsTrue(DiaRoots.Count == NewStarts.Count);
    }

    public override void initialize()
    {
        for(int i = 0; i < DiaRoots.Count; ++i)
        {
            DiaRoots[i].ModifyStarting(NewStarts[i]);
        }
        base.initialize();

        for(int i = 0; i < ExactDiaLine.Length; ++i) 
        {
            ExactDiaLine[i].GetComponent<DiaMaster>().mark_dia_for_quest(gameObject, i);
        }
    }
}
