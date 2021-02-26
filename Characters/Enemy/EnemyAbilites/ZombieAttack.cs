using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ZombieAttack : EnemyAbility
{
    [SerializeField] float damagePerSwing = 10f;
    [SerializeField] Transform thisHitbox = null;
    private List<Transform> TargetsInMeleeRange = new List<Transform>();


    protected override void Start()
    {
        base.Start();
        Assert.IsNotNull(thisHitbox);
    }

    public override void CastMechanicsForce()
    {
        for (int i = TargetsInMeleeRange.Count - 1; i >= 0; --i)
        {
            if (TargetsInMeleeRange[i].tag == "DeadEnemy")
            {
                TargetsInMeleeRange.RemoveAt(i);
            }
            else
            {
                TargetsInMeleeRange[i].GetComponent<Health>().take_damage(damagePerSwing);
            }
        }
    }

    public override bool CheckCast()
    {
        return TargetsInMeleeRange.Count > 0;
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
        else if (other.tag == "BasicEnemy" && other.transform != thisHitbox)
        {
            if (FL.ReturnIsEnemy(ETM.Return_FactionEnum(), other.gameObject.GetComponentInParent<EnemyTemplateMaster>().Return_FactionEnum()))
            {
                TargetsInMeleeRange.Add(other.transform);
            }
        }
    }
}
