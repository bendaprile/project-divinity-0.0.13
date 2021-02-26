using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HumanoidSpawner : Spawner
{
    enum enemyamount { Swarm, Group, Elite} //Do later maybe
    [SerializeField] private enemyamount EnemyAmount  = enemyamount.Group;

    private ItemCataog itemCataog;
    private int GearPoints; //Total points for the group
    private int[] PointsPerEnemy;
    

    protected override void Awake()
    {
        base.Awake();
        itemCataog = GameObject.Find("Master Object").GetComponent<ItemCataog>();
        Assert.IsFalse(Level == -1);
        SetMaxEnemy();
    }

    protected override void AdditionalSetup(int i, Transform child)
    {
        HumanoidCombatClass HCC;
        if (EnemyAmount == enemyamount.Swarm)
        {
            HCC = PickClass(false, child);
        }
        else if (EnemyAmount == enemyamount.Group)
        {
            HCC = PickClass(i % 3 == 2, child); // 1/3
        }
        else
        {
            HCC = PickClass(true, child);
        }

        SetupEnemyItems(HCC, child);
    }

    ////////////////////////////////////////////////////////////////
    private void SetMaxEnemy()
    {
        switch (EnemyAmount)
        {
            case enemyamount.Swarm:
                max_enemy = 3 + Level / 5; //All classless
                break;
            case enemyamount.Group:
                max_enemy = 2 + Level / 10; // 1/3 has a class
                break;
            case enemyamount.Elite:
                max_enemy = 1 + Level / 15; // All Classes
                break;
        }
    }


    private HumanoidCombatClass PickClass(bool SpecialClass, Transform child)
    {
        HumanoidCombatClass CombatClass = HumanoidCombatClass.Classless; //No class
        HumanoidMaster HM = child.GetComponent<HumanoidMaster>();

        if (SpecialClass)
        {
            CombatClass = (HumanoidCombatClass)Random.Range(1, 4);
        }

        HM.SetupHumanoidStats(CombatClass);
        return CombatClass;
    }


    private void SetupEnemyItems(HumanoidCombatClass HAC, Transform child)
    {
        HumanoidMaster HM = child.GetComponent<HumanoidMaster>();

        int firing_distance;
        ArmorWeightClass AWC;
        if (HAC == HumanoidCombatClass.Sharpshooter)
        {
            firing_distance = 2;
            AWC = ArmorWeightClass.Light;
        }
        else if(HAC == HumanoidCombatClass.Generalist)
        {
            firing_distance = Random.Range(0, 3); //Any
            AWC = ArmorWeightClass.Medium;
        }
        else if (HAC == HumanoidCombatClass.Tank)
        {
            firing_distance = Random.Range(0, 2); //Melee or Medium
            AWC = ArmorWeightClass.Heavy;
        }
        else
        {
            firing_distance = Random.Range(0, 3); //Any
            AWC = ArmorWeightClass.Light;
        }

        GameObject TempWeapon = itemCataog.ReturnWeaponInBudget(10, firing_distance); //TODO base the points off somthing
        (GameObject, GameObject, GameObject) TempArmor = itemCataog.ReturnArmorInBudget(10, AWC); //TODO base the points off somthing

        if (TempArmor.Item1)
        {
            (TempArmor.Item1).transform.parent = child.Find("Hitbox");
        }

        if (TempArmor.Item2)
        {
            (TempArmor.Item2).transform.parent = child.Find("Hitbox");
        }

        if (TempArmor.Item3)
        {
            (TempArmor.Item3).transform.parent = child.Find("Hitbox");
        }

        if (TempWeapon)
        {
            TempWeapon.transform.parent = child.Find("Body");
        }

        HM.SetupHumanoidItems(TempWeapon, TempArmor);
    }
}
