using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : Health
{
    protected override void Start()
    {
        base.Start();
    }

    public override void take_damage(float damage, bool PlayerIsSource = false, bool knockback = false, Vector3 force = new Vector3(), float stun_duration = 0f, DamageType DT = DamageType.Regular, bool isDoT = false)
    {
        float ModifiedDamage = HealthCalculation(damage, DT);
        health -= ModifiedDamage;

        if (healthSlider)
        {
            UpdateHealthBar();
        }
    }
}
