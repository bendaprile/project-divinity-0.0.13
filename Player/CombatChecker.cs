using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatChecker : MonoBehaviour
{
    public bool enemies_nearby;
    List<Collider> TriggerList;

    FactionLogic FL;

    private void Start()
    {
        enemies_nearby = false;
        TriggerList = transform.GetComponent<ColliderChild>().TriggerList;
        FL = FindObjectOfType<FactionLogic>();
    }

    private void FixedUpdate()
    {
        enemies_nearby = false;

        int potential_enemies_in_range = TriggerList.Count; //Includes recently dead enemies
        for (int i = potential_enemies_in_range - 1; i >= 0; i--)
        {
            Collider col = TriggerList[i];
            if (!col || col.tag != "BasicEnemy")
            {
                TriggerList.Remove(col);
            }
            else
            {
                EnemyTemplateMaster ETM = col.GetComponentInParent<EnemyTemplateMaster>();
                if (FL.ReturnIsEnemyCustomPlayer(ETM.Return_FactionEnum(), ETM.Return_customReputation()))
                {
                    enemies_nearby = true;
                }
            }

        }
    }
}
