using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereThrowingEnemy : EnemyAbility
{
    [SerializeField] private GameObject lightningSphere = null;

    [SerializeField] private float radius = 0f;

    [SerializeField] private float speed = 0f;
    [SerializeField] private float max_duration = 0f;
    [SerializeField] private float dps = 0f;
    [SerializeField] private int max_targets = 3;
    [SerializeField] private float MaintainSpeed = 0;


    public override bool CheckCast()
    {
        return(clean_LoS(true));
    }


    public override void CastMechanicsForce()
    {
        Vector3 mod_transform = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        GameObject clone = Instantiate(lightningSphere, mod_transform, transform.rotation, EnemyProjectiles).gameObject;
        Rigidbody RB = clone.GetComponent<Rigidbody>();
        LightningSphereProjectile LSP_Script = clone.GetComponentInChildren<LightningSphereProjectile>();

        RB.velocity = transform.TransformDirection(Vector3.forward * speed);

        LSP_Script.EnemySetup(FL, ETM);
        LSP_Script.GenericSetup(MaintainSpeed, dps, max_targets, max_duration, radius);

        clone.transform.Find("Sphere").GetComponent<SphereCollider>().isTrigger = true;
    }

}
