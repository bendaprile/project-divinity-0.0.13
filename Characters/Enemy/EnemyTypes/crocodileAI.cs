using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class crocodileAI : EnemyTemplateMaster
{
    [SerializeField] protected float charge_cooldown = 10f;
    [SerializeField] protected ChargingEnemy charge_script = null;

    private float next_charge = 0;

    protected override void EnemyBasicAnimation()
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float RotationSpeed = 360 * rB.velocity.magnitude / (rB.velocity.magnitude * rB.velocity.magnitude + 10); //Can only rotate at medium speeds
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, angle, 0)), RotationSpeed * Time.fixedDeltaTime);
        //animationUpdater.PlayAnimation("Blend Tree");
    }

    protected override void AIFunc()
    {
        if ((next_charge <= timer) && Current_Target && charge_script.Charge())
        {
            NormalMovement = false;
            cc_immune = true;
            next_charge = timer + charge_cooldown;
        } 
        else if(!charge_script.isCharging)
        {
            agent.SetDestination(Current_Target.position);
            NormalMovement = true;
            cc_immune = false;
        }
    }
}
