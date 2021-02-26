using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoMaster : MonoBehaviour
{
    protected bool destroyOnEnemy;
    protected int damage;
    protected DamageType dt;
    public bool Player_Fired = true;
    protected int PlayerRepModifier = 0;

    protected FactionsEnum FacEnum;
    protected FactionLogic FL;

    private bool disabled = false; //using this because destroyImmediate is bad practie

    public virtual void Setup(int dam, bool destEnem = true, DamageType dt_in = DamageType.Regular)
    {
        damage = dam;
        destroyOnEnemy = destEnem;
        dt = dt_in;
        Destroy(gameObject, 5f);
    }

    public virtual void AdditionalNPCSetup(FactionsEnum Fenum_in, FactionLogic FL_in, int PlayerRepModifier_in)
    {
        FacEnum = Fenum_in;
        FL = FL_in;
        PlayerRepModifier = PlayerRepModifier_in;
        Player_Fired = false;
    }

    public void KineticReversalHelper()
    {
        Player_Fired = true;
        GetComponent<Rigidbody>().velocity = -GetComponent<Rigidbody>().velocity;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!disabled) //Prevents projectile from damaging more than 1 object
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Terrain") || other.gameObject.layer == LayerMask.NameToLayer("Obstacles") || other.gameObject.layer ==  LayerMask.NameToLayer("SmallObstacles"))
            {
                Destroy(gameObject);
            }

            if (Player_Fired)
            {
                if (other.tag == "BasicEnemy")
                {
                    DealDamage(other, true);
                }
            }
            else
            {
                if (other.tag == "Player" && FL.ReturnIsEnemyCustomPlayer(FacEnum, PlayerRepModifier))
                {
                    DealDamage(other, false);
                }

                if (other.tag == "BasicEnemy" && FL.ReturnIsEnemy(FacEnum, other.GetComponentInParent<EnemyTemplateMaster>().Return_FactionEnum()))
                {
                    DealDamage(other, false);
                }
            }
        }
    }

    public (float, DamageType) returnDamageStats()
    {
        return (damage, dt);
    }

    protected void DealDamage(Collider other, bool PlayerCast)
    {
        other.gameObject.GetComponent<Health>().take_damage(damage, PlayerCast, DT: dt);
        if (destroyOnEnemy)
        {
            disabled = true;
            Destroy(gameObject);
        }
    }
}
