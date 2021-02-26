using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidMeleeBox : MonoBehaviour
{
    private HumanoidMaster HM;
    private FactionLogic FL;
    private EnemyTemplateMaster ETM;
    public List<Transform> TargetsInMeleeRange = new List<Transform>();


    protected virtual void Start()
    {
        HM = GetComponentInParent<HumanoidMaster>();
        ETM = GetComponentInParent<EnemyTemplateMaster>();
        FL = GameObject.Find("NPCs").GetComponent<FactionLogic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (FL.ReturnIsEnemyCustomPlayer(ETM.Return_FactionEnum(), ETM.Return_customReputation()))
            {
                TargetsInMeleeRange.Add(other.transform);
            }
        }
        else if (other.tag == "BasicEnemy")
        {
            if (FL.ReturnIsEnemy(ETM.Return_FactionEnum(), other.gameObject.GetComponentInParent<EnemyTemplateMaster>().Return_FactionEnum()))
            {
                TargetsInMeleeRange.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (FL.ReturnIsEnemyCustomPlayer(ETM.Return_FactionEnum(), ETM.Return_customReputation()))
            {
                TargetsInMeleeRange.Remove(other.transform);
            }
        }
        else if (other.tag == "BasicEnemy" && other.transform != HM.Return_selfTrans())
        {
            if (FL.ReturnIsEnemy(ETM.Return_FactionEnum(), other.gameObject.GetComponentInParent<EnemyTemplateMaster>().Return_FactionEnum()))
            {
                TargetsInMeleeRange.Add(other.transform);
            }
        }
    }
}
