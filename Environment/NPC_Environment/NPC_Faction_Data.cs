using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NPC_Faction_Data : MonoBehaviour
{
    [SerializeField] private List<Transform> External_Activity_Locs_single = new List<Transform>();
    [SerializeField] private List<Transform> External_Activity_Locs_DoubleArray = new List<Transform>(); //Must be empty transforms that each contain Activities
    [SerializeField] private List<ActivityBasedDia> ABD = new List<ActivityBasedDia>();

    [SerializeField] [TextArea(3, 10)] private List<string> FactionDia = new List<string>();

    private List<Transform> Activity_Locs = new List<Transform>();
    private Queue<string> FactionDiaPriority = new Queue<string>();
    private int iter = 0;

    private void Start()
    {
        Activity_Locs.AddRange(External_Activity_Locs_single);

        for (int i = 0; i < External_Activity_Locs_DoubleArray.Count; ++i)
        {
            for (int j = 0; j < External_Activity_Locs_DoubleArray[i].childCount; ++j)
            {
                Activity_Locs.Add(External_Activity_Locs_DoubleArray[i].GetChild(j));
            }
        }
    }

    public List<Transform> Return_Activity_Locs()
    {
        return Activity_Locs;
    }


    public void AddDiaPri(string str)
    {
        FactionDiaPriority.Enqueue(str);
    }

    public string Return_Line(NPCActivityFlag npcAF)
    {
        if (FactionDiaPriority.Count > 0)
        {
            return FactionDiaPriority.Dequeue();
        }
        
        
        if(npcAF != NPCActivityFlag._NO_FLAG_)
        {
            for(int i = 0; i < ABD.Count; ++i)
            {
                if(npcAF == ABD[i].npcAF)
                {
                    return ABD[i].Return_Line();
                }
            }
        }


        string final_line = FactionDia[iter];
        iter = (iter + 1) % FactionDia.Count;
        return final_line;
    }
}
