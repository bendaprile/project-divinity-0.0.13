using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightningCrocodileAI : EnemyTemplateMaster
{
    [SerializeField] protected float sphere_cast_cooldown = 10f;
    [SerializeField] protected float lightning_cast_cooldown = 10f;
    [SerializeField] protected SphereThrowingEnemy sphere_script = null;
    [SerializeField] protected LightningCastingEnemy lightning_script = null;

    private float sphere_next_cast;
    private float lightning_next_cast;


    protected override void Start()
    {
        base.Start();
        sphere_next_cast = 0;
        lightning_next_cast = 0;
    }

    protected override void EnemyBasicAnimation()
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float RotationSpeed = 360 * rB.velocity.magnitude / (rB.velocity.magnitude * rB.velocity.magnitude + 10); //Can only rotate at medium speeds
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, angle, 0)), RotationSpeed * Time.fixedDeltaTime);
        //animationUpdater.PlayAnimation("Blend Tree");
    }

    protected override void AIFunc()
    {
        if ((sphere_next_cast <= timer) && sphere_script.CheckCast())
        {
            animationUpdater.PlayBlockingAnimation("sphere_attack");
            sphere_next_cast = timer + sphere_cast_cooldown;
        }
        else if (lightning_next_cast <= timer && lightning_script.CastMechanics())
        {
            lightning_next_cast = timer + lightning_cast_cooldown;
        }
        else
        {
            base.AIFunc();
        }
    }

    public override void AnimationCalledFunc0() //Fires in the middle of the animation
    {
        sphere_script.CastMechanicsForce();
    }
}
