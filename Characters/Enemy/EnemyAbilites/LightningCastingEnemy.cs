using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCastingEnemy : EnemyAbility
{
    [SerializeField] private GameObject lightningBolt = null;
    [SerializeField] private float y_adjust = -1.2f;

    [SerializeField] private int burst_amount = 0;
    [SerializeField] private float scatter_range = 0f;
    [SerializeField] private float damage = 0f;
    [SerializeField] private float AttackDelay = 0f;
    [SerializeField] private float radius = 0;

    public override bool CastMechanics()
    {
        if (target_in_range())
        {
            StartCoroutine(attackMechanics());
            return true;
        }
        return false;
    }

    private IEnumerator attackMechanics()
    {
        for(int i = 0; i < burst_amount; i++)
        {
            Vector3 cast_pos = new Vector3(ETM.Return_Current_Target().position.x + Random.Range(-scatter_range, scatter_range), ETM.Return_Current_Target().position.y + y_adjust, ETM.Return_Current_Target().position.z + Random.Range(-scatter_range, scatter_range));

            GameObject clone = Instantiate(lightningBolt, cast_pos, transform.rotation, EnemyProjectiles).gameObject;
            LightningBoltProjectile LBP = clone.GetComponent<LightningBoltProjectile>();
            LBP.EnemySetup(FL, ETM);
            LBP.GenericSetup(damage, radius, AttackDelay);
            yield return new WaitForSeconds(.1f);
        }
    }
}
