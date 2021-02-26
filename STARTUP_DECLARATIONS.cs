using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


///General Damage
public enum DamageType { True, Piercing, Regular }
public enum DamageSource { Biomechanical, Projectile, Lacerate}
///General Damage

///Misc
public enum MoveDir { Forward, Left, Right, Backward }
///Misc

///Player
public enum CoreIntrinsicSkillsEnum { Strength, Dexterity, Aptitude, Intelligence, Toughness, Willpower }
public enum DerivedIntrinsicSkillsEnum { Vigor, Cerebral, Fortitude }
public enum SkillsEnum { Larceny, Science, Medicine, Engineering, Speech, Survival, Perception }
public enum CombatSkillsEnum { ranged_proficiency, melee_proficiency, armor_proficiency, athletic_proficiency }
public enum MoveState { Idle, Walking, Running, Rolling, ForceBased, FollowCursor }
public enum EventTypeEnum { QuestObjCompleted, QuestCompleted, QuestFailed, QuestStarted, LevelUp, AbilityLevelUp }
public enum ExclusiveActivePerksEnum { RollReload, LightningSphereTurret, InstantTele, ProjShieldReflection }
public enum AttributesEnum
{
    max_energy, energy_regen, max_stamina, stamina_regen, max_health, health_regen, armor, plating,
    xp_gain, lightning_dmg, lacerate_dmg, piercing_dmg
}
///Player

///Items
public enum ItemTypeEnum { Misc, Armor, Weapon, Consumable, Implant }
public enum WeaponType { Melee_1H, Melee_2H, Ranged }
public enum ArmorWeightClass { Light, Medium, Heavy }
public enum ArmorType { Head, Chest, Legs }
public enum ConsumableType { Restoring, Grenade }
public enum ItemQuality { Damaged, Flawed, Common, Rare, Flawless, Legendary }
public enum RangedAnimation { Ranged_1H, Ranged_2H, Minigun, Launcher }
public enum MeleeAnimation { StandingAttackDownward, StandingAttack360High, JumpAttack }
///Items

///Abilites
public enum AbilityType { Lightning, Spacetime, Exoskeleton, Regeneration, Faction }
public enum AbilityHolderEnum
{
    LigSphThr, LigBolCas, LigBlaCas, LigMel,
    TimDia, ProShiCas, Tel, KinRev,

    InsHea
}
public enum AbilityAnimation
{
    ThrowArmsForward, Battlecry, ThrowArmForward, UnderhandThrowArmUp, ExplodeArmsOutward, SwingBackThrowArmsForward,
    HoldHandsForward, BendDownThrowArmsForward_FB, TwistSlamDown_FB
}
///Abilites

///Quest
public enum ObjectiveType { NOTHING, Fetch, GenericKill, SpawnerKill, Talk }
public enum QuestCategory { Main, Side, Miscellaneous, Completed }
///Quest

///NPC/Humanoid
public enum HumanoidLogicMode { none, Kiting, Kiting_reposition, Charge_in, Flee }
public enum HumanoidCombatClass { Classless, Sharpshooter, Generalist, Tank, Antagonist }
public enum HumanoidWeaponExpertise { Novice, Adept, Commando }
public enum HumanoidMovementType { Hindered, Average, Agile, inhuman }
public enum NPC_Control_Mode { WalktoPlayer_dia, Scripted, NPC_control, Enemy_control }

public enum NPCActivityFlag { _NO_FLAG_, 
    City_ArenaSpectator, City_Bench,
                        } //Used for activity based Dia

public enum NPC_FactionsEnum
{
    CityRich, CityPoor,
    Plantation_Farmer
}
///NPC/Humanoid

public enum FactionsEnum{ Player, Rogue, Feral, AntiPlayer, B, C, Plantation, MidwayCityCivilian }

public enum Zones { Apoc, Water, Snow, Jungle, City }

public static class STARTUP_DECLARATIONS
{
    public const int NPC_FactionsCount = 3;
    public const int FactionCount = 8;
    public const int AllyNumber = 1800;
    public const int EnemyNumber = 200;

    public const int Number_of_ExclusiveActivePerks = 4;
    public const int Number_of_Attributes = 12;
    public const int Number_of_GeneralSkills = 7;
    public const int Number_of_CombatSkills = 4;
    public const float TIME_TO_DISPLAY_TOOLTIP = 0.1f;

    public static string[] FactionsEnumReverse = new string[FactionCount] { "Ally", "Rogue", "Feral", "AntiPlayer", "B", "C", "Plantation", "Midway City Civilian" };

    public static string[] NPC_FactionsEnumReverse = new string[NPC_FactionsCount] { "City Rich", "City Poor", "Terry Farmer" };

    public static string[] SkillEnumReverse = new string[7] { "Larceny", "Science", "Medicine", "Engineering", "Speech", "Survival", "Perception" };
    public static CoreIntrinsicSkillsEnum[] Skill_CoreTie = new CoreIntrinsicSkillsEnum[7] { CoreIntrinsicSkillsEnum.Dexterity,
        CoreIntrinsicSkillsEnum.Intelligence, CoreIntrinsicSkillsEnum.Intelligence, CoreIntrinsicSkillsEnum.Intelligence, CoreIntrinsicSkillsEnum.Intelligence,
        CoreIntrinsicSkillsEnum.Toughness, CoreIntrinsicSkillsEnum.Willpower };

    public static string[] CombatSkillsEnumReverse = new string[4] { "ranged_proficiency", "melee_proficiency", "armor_proficiency", "athletic_proficiency" };
    public static CoreIntrinsicSkillsEnum[] CombatSkills_CoreTie = new CoreIntrinsicSkillsEnum[4] { CoreIntrinsicSkillsEnum.Dexterity, CoreIntrinsicSkillsEnum.Strength,
        CoreIntrinsicSkillsEnum.Toughness, CoreIntrinsicSkillsEnum.Willpower };

    public static AttributesEnum[] DamageSource_AttributesTie = { AttributesEnum.lightning_dmg, AttributesEnum.lacerate_dmg, AttributesEnum.piercing_dmg };

    public static string[] ArmorTypeEnumReverse = new string[3] { "Head", "Chest", "Legs"};
    public static string[] ArmorWeightEnumReverse = new string[3] { "Light", "Medium", "Heavy" };
    public static string[] ItemClassEnumReverse = new string[6] { "Damaged", "Flawed", "Common", "Rare", "Flawless", "Legendary" };
    public static string[] WeaponTypeEnumReverse = new string[3] { "Melee 1-handed", "Melee 2-handed", "Ranged"};
    public static string[] DamageTypeEnumReverse = new string[3] { "True" , "Elemental", "Regular" };
    public static string[] DamageSourceEnumReverse = new string[3] { "Lightning", "Projectile", "Lacerate" };

    public static Color32 goldColor = new Color32(255, 199, 0, 255);
    public static Color32 goldColorTransparent = new Color32(255, 199, 0, 100);

    public static float[] AbilityAnimationCastTime = new float[9] { .8f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f };

    public static Dictionary<ItemQuality, Color32> itemQualityColors = new Dictionary<ItemQuality, Color32>
    {
        { ItemQuality.Damaged, new Color32(159, 159, 159, 255) },
        { ItemQuality.Flawed, new Color32(112, 255, 0, 255) },
        { ItemQuality.Common, new Color32(0, 131, 255, 255) },
        { ItemQuality.Rare, new Color32(170, 0, 255, 255) },
        { ItemQuality.Flawless, new Color32(255, 192, 0, 255) },
        { ItemQuality.Legendary, new Color32(0, 255, 255, 255) }
    };

    public static (string, string)[] skillsDescriptions = new (string, string)[20]
    {
        ////////////// GENERAL SKILLS
        ("Larceny", "Larceny is used for unlocking doors for places where you do not belong. Your initial value here is affected directly by your Dexterity."),
        ("Science", "Your initial value here is affected directly by your Intelligence."),
        ("Medicine", "Your initial value here is affected directly by your Intelligence."),
        ("Engineering", "Your initial value here is affected directly by your Intelligence."),
        ("Speech", "Your initial value here is affected directly by your Intelligence."),
        ("Survival", "Your initial value here is affected directly by your Toughness."),
        ("Perception", "Your initial value here is affected directly by your Willpower."),

        /////////////// COMBAT SKILLS
        ("Ranged Proficiency", "Ranged Proficiency impacts how effective you are with ranged weapons such as guns. A higher number here means you will do more damage with all ranged weapons (does not affect abilities). Your initial value here is affected directly by your Dexterity."),
        ("Melee Proficiency", "Melee Proficiency impacts how effective you are with melee weapons such as axes and swords. A higher number here means you will do more damage with all melee weapons (does not affect abilities)."
        + ParagraphBreak() + " Your initial value here is affected directly by your Strength. This means that the higher your strength value, the higher your Melee Proficiency can be."),
        ("Armor Proficiency", "Armor Proficiency impacts how effective you are with the armor you wear. Your initial value here is affected directly by your Toughness."),
        ("Athletic Proficiency", "Athletic Proficiency impacts how effective you are at dodge rolling and sprinting. A higher number here means you will use less energy when rolling and sprinting. Your initial value here is affected directly by your Willpower."),

        ("Vigor", ""),
        ("Strength", ""),
        ("Dexterity", ""),
        ("Cerebral", ""),
        ("Aptitude", ""),
        ("Intelligence", ""),
        ("Fortitude", ""),
        ("Toughness", ""),
        ("Willpower", ""),
    };


    public static (string, string)[] statsDescriptions = new (string, string)[Number_of_Attributes]
    {
        ("Energy", "The maximum amount of energy that you can have at anypoint. Most abilities can only use energy."),
        ("Energy Regen", "The rate at which your energy is restored naturally."),
        ("Stamina", "The maximum amount of Stamina that you can have at anypoint. Advanced movement and weapons use Stamina before Energy."),
        ("Stamina Regen", "The rate at which your Stamina is restored natrually."),
        ("Health", "The maximum amount of damage you take before dying. Plating and Armor can reduce the amount of damage you take."),
        ("Health Regen", "The rate at which you natrually restore health."),
        ("Armor", "Reduces the amount of damage you take by a percentage (calculated after plating). The damage reduction is calculated by (Armor) / (Armor + 100). True damage will ignore Armor."),
        ("Plating", "Reduces the amount of damage you take by a flat amount (calculated before Armor). True damage and elemental damage will ignore plating."),
        ("EXP Multiplier", "Increases the amount of EXP gained."),
        ("Biomechanical Multiplier", "Increases all Biomechanical damage (mainly abilites) by the percentage below."),
        ("Lacerate Multiplier", "Increases all Lacerate damage (mainly melee weapons) by the percentage below."),
        ("Projectile Multiplier", "Increases all Projectile damage (mainly ranged weapons) by the percentage below.")
    };

    public static string ParagraphBreak()
    {
        return Environment.NewLine + Environment.NewLine;
    }
}
