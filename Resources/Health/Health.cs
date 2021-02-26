using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 0;
    [SerializeField] protected float healthRegen = 0;
    [SerializeField] protected int Plating = 0;
    [SerializeField] protected int Armor = 0;

    [SerializeField] protected GameObject[] BloodArray;
    [SerializeField] protected Image healthSlider = null;

    private int BloodIter;
    protected Transform NonLogicProjectiles;
    protected float health;

    protected float ModifiedDamage; //here so override functions can use it

    protected virtual void Start()
    {
        BloodIter = Random.Range(0, BloodArray.Length);
        NonLogicProjectiles = GameObject.Find("NonLogicProjectiles").transform;
        health = maxHealth;
        if (healthSlider)
        {
            UpdateHealthBar();
        }
    }

    public void modify_maxHealth(float value)
    {
        maxHealth = value;
        health += value;
        if (healthSlider)
        {
            UpdateHealthBar();
        }
    }

    public float ReturnMaxHealth()
    {
        return maxHealth;
    }

    public void modify_healthRegen(float value)
    {
        healthRegen = value;
    }

    public void modify_Plating(int value)
    {
        Plating = value;
    }

    public void modify_Armor(int value)
    {
        Armor = value;
    }

    public virtual void take_damage(float damage, bool PlayerIsSource = false, bool knockback = false, Vector3 force = new Vector3(), float stun_duration = 0f, DamageType DT = DamageType.Regular, bool isDoT = false)
    {
        ModifiedDamage = HealthCalculation(damage, DT);
        health -= ModifiedDamage;

        if (!isDoT && BloodArray.Length > 0)
        {
            GameObject Bloodtemp = Instantiate(BloodArray[BloodIter], NonLogicProjectiles);
            Bloodtemp.transform.position = transform.position;
            Bloodtemp.transform.rotation = transform.rotation;
            Bloodtemp.GetComponent<BFX_BloodSettings>().SetFollow(transform);
            Destroy(Bloodtemp, 20f);
            BloodIter = (BloodIter + 1) % BloodArray.Length;
        }

        if (healthSlider)
        {
            UpdateHealthBar();
        }
    }

    public float ReturnCurrentHealth()
    {
        return health;
    }

    public void heal(float amount)
    {
        if(health < maxHealth)
        {
            health += amount;
        }

        if (healthSlider)
        {
            UpdateHealthBar();
        }
    }

    protected float HealthCalculation(float damage, DamageType DT)
    {
        if (DT == DamageType.Regular)
        {
            damage -= Plating;
        }

        if (DT == DamageType.Regular || DT == DamageType.Piercing)
        {
            float resist = (100f / (Armor + 100f));
            damage *= resist;
        }

        if(damage < 0)
        {
            damage = 0;
        }

        return damage;
    }

   protected void UpdateHealthBar()
    {
        healthSlider.fillAmount = health / maxHealth;
    }

   protected virtual void FixedUpdate()
    {
        heal(healthRegen * Time.fixedDeltaTime);

        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
