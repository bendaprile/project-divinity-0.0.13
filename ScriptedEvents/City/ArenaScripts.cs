using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScripts : MonoBehaviour
{
    [SerializeField] private Transform ArenaRival;
    [SerializeField] private Transform Rival_dia_post_fight1;
    [SerializeField] private GameObject Fight1_flag;


    private Zone_Flags ZF;

    void Start()
    {
        ZF = FindObjectOfType<Zone_Flags>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Fight1_flag && ZF.CheckFlag(Fight1_flag.name))
        {
            Fight1_flag = null;
            ArenaRival.GetComponentInChildren<DiaRoot>().ModifyStarting(Rival_dia_post_fight1);
            ArenaRival.GetComponent<HumanoidMaster>().Set_ControlMode(NPC_Control_Mode.WalktoPlayer_dia);
        }
    }
}
