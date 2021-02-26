using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Logic : MonoBehaviour
{
    [SerializeField] private NPC_Faction_Data[] Total_Faction_Data = new NPC_Faction_Data[STARTUP_DECLARATIONS.NPC_FactionsCount];

    public Transform FindDest(List<NPC_FactionsEnum> facs, List<int> upperBounds)
    {
        NPC_FactionsEnum NPC_FacEnum = FactionPicker(facs, upperBounds); //Pick Faction
        List<Transform> Activity_Locs = Total_Faction_Data[(int)NPC_FacEnum].Return_Activity_Locs(); //Get activities

        int index = Random.Range(0, Activity_Locs.Count); //Find next
        return Activity_Locs[index];
    }

    public string ReturnFactionLine(NPCActivityFlag npcAF, List<NPC_FactionsEnum> facs, List<int> upperBounds)
    {
        NPC_FactionsEnum NPC_FacEnum = FactionPicker(facs, upperBounds); //Pick Faction

        return Total_Faction_Data[(int)NPC_FacEnum].Return_Line(npcAF);
    }


    private NPC_FactionsEnum FactionPicker(List<NPC_FactionsEnum> facs, List<int> upperBounds)
    {
        int rand = Random.Range(0, 100);
        int iter = 0;

        while (rand > upperBounds[iter])
        {
            iter += 1;
        }

        return facs[iter];
    }
}