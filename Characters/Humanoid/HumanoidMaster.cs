using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HumanoidMaster : MonoBehaviour
{
    [SerializeField] private HumanoidCombatClass CombatClass = HumanoidCombatClass.Classless;
    [SerializeField] private NPC_Control_Mode Control_Mode = NPC_Control_Mode.NPC_control; //If a non enemyControl is attacked, this will not change

    [SerializeField] public NPC npc = null; //This is used as a reference for other scripts

    private NPC_Control_Mode Prev_Frame_Control_Mode = NPC_Control_Mode.NPC_control;

    private Transform selfTrans;
    private HumanoidEnemy HE;
    private EnemyWeaponController EWC;
    private EnemyAnimationUpdater EAU;
    private EnemyArmor EA;
    private InteractiveDia ID;
    private EnemyHealth EH;
    private DiaRoot DR;

    private bool AnimationOverride = false;

    //////////////////Key Stats
    HumanoidMovementType MovementType = HumanoidMovementType.Average;
    private HumanoidWeaponExpertise WeaponExpertise = HumanoidWeaponExpertise.Novice;
    //////////////////Key Stats


    void Awake()
    {
        HE = GetComponent<HumanoidEnemy>();
        EWC = GetComponentInChildren<EnemyWeaponController>();
        EAU = GetComponentInChildren<EnemyAnimationUpdater>();
        EA = GetComponentInChildren<EnemyArmor>();
        ID = GetComponentInChildren<InteractiveDia>();
        EH = GetComponentInChildren<EnemyHealth>();
        DR = GetComponentInChildren<DiaRoot>();
        selfTrans = transform.Find("Hitbox");
    }

    private void Start()
    {
        ClassBasedStats();
    }

    public void SetupHumanoidStats(HumanoidCombatClass CombatClass_in)
    {
        CombatClass = CombatClass_in;
        ClassBasedStats();
    }

    private void FixedUpdate()
    {
        Assert.IsFalse(Control_Mode == NPC_Control_Mode.Enemy_control);
        if(Return_Control_Mode() == NPC_Control_Mode.WalktoPlayer_dia)
        {
            MoveToDest(HE.player.transform.position, 6f);

            float distance = Vector3.Distance(HE.player.transform.position, transform.position);
            if(distance < 5)
            {
                ID.ForcedActivate();
                Set_ControlMode(NPC_Control_Mode.NPC_control);
            }
        }
    }

    public void Set_ControlMode(NPC_Control_Mode mode)
    {
        Control_Mode = mode;
        Control_Mode_Change_Logic();
    }


    public void SetupHumanoidItems(GameObject Weapon, (GameObject, GameObject, GameObject) Armor)
    {
        EWC.UpdateWeaponSlot(Weapon);
        EA.AttachArmor(Armor.Item1, Armor.Item2, Armor.Item3);
    }

    public void Start_HumanoidAttackMode()
    {
        AnimationOverride = false;
        ID.CombatText(.8f);
        EAU.Set_is2hRanged(EWC.ReturnWeapon());
    }

    public void Attempt_CombatModeRevert()
    {
        ID.NormalText();
        EAU.Set_is2hRanged(EWC.ReturnWeapon());
        Control_Mode_Change_Logic();
    }

    private void Control_Mode_Change_Logic() //TODO and other logic for other modes if needed
    {
        npc.RandomTask();
    }

    public void MoveToDest(Vector3 dest, float speed = 3f)
    {
        HE.ExternalMovement(dest, speed);
    }

    public void ExternalAnimation(bool start = true, string anim = "")
    {
        AnimationOverride = start;
        if (start)
        {
            EAU.PlayAnimation(anim);
        }
    }


    ////////////////////////////////////////
    public NPC_Control_Mode Return_Control_Mode()
    {
        if (HE.return_AIEnabled())
        {
            return NPC_Control_Mode.Enemy_control;
        }
        return Control_Mode;
    }

    public EnemyAnimationUpdater Return_Animation_Updater()
    {
        return EAU;
    }

    public bool return_AnimationOverride()
    {
        return AnimationOverride;
    }

    public Transform Return_selfTrans()
    {
        return selfTrans;
    }

    public HumanoidWeaponExpertise Return_WeaponExpertise()
    {
        return WeaponExpertise;
    }

    public HumanoidCombatClass Return_CombatClass()
    {
        return CombatClass;
    }

    public HumanoidMovementType Return_MovementType()
    {
        return MovementType;
    }

    public string Return_Name()
    {
        return DR.ReturnNPC_name();
    }
    ////////////////////////////////////////

    private void ClassBasedStats()
    {
        int max_hp;
        int xp_val;
        HumanoidWeaponExpertise HWE_temp;
        string name = STARTUP_DECLARATIONS.FactionsEnumReverse[(int)HE.Return_FactionEnum()];

        switch (CombatClass)
        {
            case HumanoidCombatClass.Antagonist:
                HWE_temp = HumanoidWeaponExpertise.Commando;
                MovementType = HumanoidMovementType.inhuman;
                xp_val = 2000;
                max_hp = 600;
                break;
            case HumanoidCombatClass.Sharpshooter:
                name += " Sharpshooter";
                HWE_temp = HumanoidWeaponExpertise.Commando;
                MovementType = HumanoidMovementType.Average;
                xp_val = 300;
                max_hp = 150;
                break;
            case HumanoidCombatClass.Generalist:
                name += " Generalist";
                HWE_temp = HumanoidWeaponExpertise.Adept;
                MovementType = HumanoidMovementType.Agile;
                xp_val = 300;
                max_hp = 200;
                break;
            case HumanoidCombatClass.Tank:
                name += " Tank";
                HWE_temp = HumanoidWeaponExpertise.Novice;
                MovementType = HumanoidMovementType.Hindered;
                xp_val = 300;
                max_hp = 300;
                break;
            default:
                name += " Recruit";
                HWE_temp = HumanoidWeaponExpertise.Novice;
                MovementType = HumanoidMovementType.Average;
                xp_val = 100;
                max_hp = 150;
                break;
        }

        if(DR.ReturnNPC_name() != "")
        {
            name = DR.ReturnNPC_name();
        }

        HE.Set_exp_reward(xp_val);
        EH.modify_maxHealth(max_hp);
        ID.SetText(name);
        WeaponExpertise = HWE_temp;
    }
}
