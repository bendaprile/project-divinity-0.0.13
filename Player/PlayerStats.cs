using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerStats : MonoBehaviour
{
    [SerializeField] private string playerName = "No Name Set";
    [SerializeField] private Transform EXPUI = null;
    private Image EXPBar;
    private Image EXPBarGhost;

    private AbilitiesController abilitiesController;
    private GameObject Player;
    private Health PlayerHealth;
    private Energy PlayerEnergy;
    private CombatChecker combatChecker;
    private UIController uIController;
    private EventQueue eventQueue;

    //////////////////////////////////////
    private int level;
    private int current_exp;
    private int ImplantPoints;
    private bool LevelUpQued = false;

    private float FinishedBarDuration = 0;


    public void AddEXP(int exp_in)
    {
        exp_in = (int)(exp_in * (((ReturnAttribute(AttributesEnum.xp_gain) * .01f) + 1)));
        abilitiesController.DistributeXP(exp_in);
        current_exp += (exp_in);
        FinishedBarDuration = 2;
        EXPBarGhost.fillAmount = Mathf.Min((float)current_exp / (level * 1000), 1f);
        EXPUI.gameObject.SetActive(true);
        EXPUI.Find("EXPNum").GetComponent<TextMeshProUGUI>().text = "+" + exp_in.ToString();
        EXPUI.Find("CharacterLevel").GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL " + level;
    }

    private void UpdateDisplayXP()
    {
        if(FinishedBarDuration > 0)
        {

            if (EXPBar.fillAmount < EXPBarGhost.fillAmount)
            {
                EXPBar.fillAmount += .004f;
            }
            else if(EXPBarGhost.fillAmount == 1)
            {
                LevelUp();
                EXPBarGhost.fillAmount = Mathf.Min((float)current_exp / (level * 1000), 1f);
                EXPBar.fillAmount = 0f;
                EXPUI.Find("CharacterLevel").GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL " + level;
            }
            else
            {
                FinishedBarDuration -= Time.deltaTime;
            }
        }
        else
        {
            EXPUI.gameObject.SetActive(false);

        }
    }

    public int returnLevel()
    {
        return level;
    }

    public string getPlayerName()
    {
        return playerName;
    }

    public int returnFreeSkillPoints()
    {
        return (5 + ReturnDerivedIntrinsicSkill(DerivedIntrinsicSkillsEnum.Cerebral) / 4);
    }

    public int returnImplantPoints()
    {
        return (ImplantPoints);
    }

    public void modifyImplantPoints(int change)
    {
        ImplantPoints += change;
    }

    private void LevelUp()
    {
        if (!LevelUpQued)
        {
            current_exp -= level * 1000;
            LevelUpQued = true;

            /////
            EventData tempEvent = new EventData();
            tempEvent.Setup(EventTypeEnum.LevelUp, "");
            eventQueue.AddEvent(tempEvent);
            /////
        }

        if (!combatChecker.enemies_nearby)
        {
            EXPBar.fillAmount = 0f;
            modifyImplantPoints(3);
            level += 1;

            uIController.LevelUpMenuBool(true);
            LevelUpQued = false;
        }
    }

    //////////////////////////////////////



    //////////////////////////////////////
    private int[] CoreStatsStorage = new int[6];
    private int[] DerivedStatsStorage = new int[3];

    public void SetCoreIntrinsicSkill(CoreIntrinsicSkillsEnum CoreIntrinsicSkill, int value)
    {
        CoreStatsStorage[(int)CoreIntrinsicSkill] = value;
        UpdateDerivedStats();
        UpdateAllSkills();
        UpdateBaseAttributes();
    }

    public int ReturnCoreIntrinsicSkill(CoreIntrinsicSkillsEnum CoreIntrinsicSkill)
    {
        return CoreStatsStorage[(int)CoreIntrinsicSkill];
    }

    public int ReturnDerivedIntrinsicSkill(DerivedIntrinsicSkillsEnum DerivedIntrinsicSkill)
    {
        return DerivedStatsStorage[(int)DerivedIntrinsicSkill];
    }

    private void UpdateDerivedStats()
    {
        DerivedStatsStorage[(int)DerivedIntrinsicSkillsEnum.Vigor] = (CoreStatsStorage[(int)CoreIntrinsicSkillsEnum.Strength] + CoreStatsStorage[(int)CoreIntrinsicSkillsEnum.Dexterity]) / 2;
        DerivedStatsStorage[(int)DerivedIntrinsicSkillsEnum.Cerebral] = (CoreStatsStorage[(int)CoreIntrinsicSkillsEnum.Aptitude] + CoreStatsStorage[(int)CoreIntrinsicSkillsEnum.Intelligence]) / 2;
        DerivedStatsStorage[(int)DerivedIntrinsicSkillsEnum.Fortitude] = (CoreStatsStorage[(int)CoreIntrinsicSkillsEnum.Toughness] + CoreStatsStorage[(int)CoreIntrinsicSkillsEnum.Willpower]) / 2;
    }
    //////////////////////////////////////



    //////////////////////////////////////
    private float[] FinalAttributesStorage = new float[STARTUP_DECLARATIONS.Number_of_Attributes];
    public List<(string, float)>[] AttributesAdditiveEffects = new List<(string, float)>[STARTUP_DECLARATIONS.Number_of_Attributes];
    public List<(string, float)>[] AttributesMultiplicativeEffects = new List<(string, float)>[STARTUP_DECLARATIONS.Number_of_Attributes];

    public void UpdateBaseAttributes()
    {
        AddAttributeEffect(AttributesEnum.max_health, "Base", true, 100);
        AddAttributeEffect(AttributesEnum.max_health, "Fortitude", true, level * ReturnDerivedIntrinsicSkill(DerivedIntrinsicSkillsEnum.Fortitude) * 2);
        AddAttributeEffect(AttributesEnum.health_regen, "Base", true, .1f);

        AddAttributeEffect(AttributesEnum.max_energy, "Base", true, 150);
        AddAttributeEffect(AttributesEnum.max_energy, "Vigor", true, level * ReturnDerivedIntrinsicSkill(DerivedIntrinsicSkillsEnum.Vigor) * 1.5f);
        AddAttributeEffect(AttributesEnum.max_stamina, "Base", true, 50);
        AddAttributeEffect(AttributesEnum.max_stamina, "Vigor", true, level * ReturnDerivedIntrinsicSkill(DerivedIntrinsicSkillsEnum.Vigor) * .5f);

        AddAttributeEffect(AttributesEnum.energy_regen, "Base", true, 5f);
        AddAttributeEffect(AttributesEnum.energy_regen, "Athletic Proficiency", false, ReturnCombatSkill(CombatSkillsEnum.athletic_proficiency) * .01f);
        AddAttributeEffect(AttributesEnum.stamina_regen, "Base", true, 15f);
        AddAttributeEffect(AttributesEnum.stamina_regen, "Athletic Proficiency", false, ReturnCombatSkill(CombatSkillsEnum.athletic_proficiency) * .01f);

        AddAttributeEffect(AttributesEnum.armor, "Armor Proficiency", true, ReturnCombatSkill(CombatSkillsEnum.armor_proficiency) * 2);

        AddAttributeEffect(AttributesEnum.xp_gain, "Aptitude", true, (float)ReturnCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Aptitude) * 2.5f);

        AddAttributeEffect(AttributesEnum.lacerate_dmg, "Melee Proficiency", true, ReturnCombatSkill(CombatSkillsEnum.melee_proficiency) * 2f);

        AddAttributeEffect(AttributesEnum.piercing_dmg, "Ranged Proficiency", true, ReturnCombatSkill(CombatSkillsEnum.ranged_proficiency) * 2f);
    }

    public float ReturnAttribute(AttributesEnum AttributeName)
    {
        return FinalAttributesStorage[(int)AttributeName];
    }

    public void AddAttributeEffect(AttributesEnum AttributeName, string EffectName, bool isAdd, float value) //Overwrites effect with the same name
    {
        if (isAdd)
        {
            for(int location = 0; location < AttributesAdditiveEffects[(int)AttributeName].Count; ++location)
            {
                if(AttributesAdditiveEffects[(int)AttributeName][location].Item1 == EffectName)
                {
                    AttributesAdditiveEffects[(int)AttributeName][location] = (EffectName, value);
                    RecalculateAttribute((int)AttributeName);
                    return;
                }
            }
            AttributesAdditiveEffects[(int)AttributeName].Add((EffectName, value));
        }
        else
        {
            for (int location = 0; location < AttributesMultiplicativeEffects[(int)AttributeName].Count; ++location)
            {
                if (AttributesMultiplicativeEffects[(int)AttributeName][location].Item1 == EffectName)
                {
                    AttributesMultiplicativeEffects[(int)AttributeName][location] = (EffectName, value);
                    RecalculateAttribute((int)AttributeName);
                    return;
                }
            }
            AttributesMultiplicativeEffects[(int)AttributeName].Add((EffectName, value));
        }
        RecalculateAttribute((int)AttributeName);
    }

    public void RemoveAttributeEffect(AttributesEnum AttributeName, string EffectName, bool isAdd)
    {
        if (isAdd)
        {
            int len = AttributesAdditiveEffects[(int)AttributeName].Count;
            for (int i = 0; i < len; i++)
            {
                if (AttributesAdditiveEffects[(int)AttributeName][i].Item1 == EffectName)
                {
                    AttributesAdditiveEffects[(int)AttributeName].RemoveAt(i);
                    break;
                }
            }
        }
        else
        {
            int len = AttributesMultiplicativeEffects[(int)AttributeName].Count;
            for (int i = 0; i < len; i++)
            {
                if (AttributesMultiplicativeEffects[(int)AttributeName][i].Item1 == EffectName)
                {
                    AttributesMultiplicativeEffects[(int)AttributeName].RemoveAt(i);
                    break;
                }
            }
        }
        RecalculateAttribute((int)AttributeName);
    }

    public List<(string, float)> ReturnAttributeEffects(AttributesEnum AttributeName, bool isAdd)
    {
        if (isAdd)
        {
            return AttributesAdditiveEffects[(int)AttributeName];
        }
        else
        {
            return AttributesMultiplicativeEffects[(int)AttributeName];
        }
    }

    private void RecalculateAttribute(int AttributeLoc)
    {
        float temp = 0;

        foreach ((string, float) addTemp in AttributesAdditiveEffects[AttributeLoc])
        {
            temp += addTemp.Item2;
        }

        float MultModifier = 1;
        foreach ((string, float) multTemp in AttributesMultiplicativeEffects[AttributeLoc])
        {
            MultModifier += multTemp.Item2;
        }

        temp *= MultModifier;

        FinalAttributesStorage[AttributeLoc] = temp;
        UpdateExternalAttributes();
    }

    private void UpdateExternalAttributes()
    {
        PlayerHealth.modify_maxHealth(ReturnAttribute(AttributesEnum.max_health));
        PlayerHealth.modify_healthRegen(ReturnAttribute(AttributesEnum.health_regen));
        PlayerHealth.modify_Armor((int)ReturnAttribute(AttributesEnum.armor));
        PlayerHealth.modify_Plating((int)ReturnAttribute(AttributesEnum.plating));
        PlayerEnergy.modify_maxEnergy(ReturnAttribute(AttributesEnum.max_energy));
        PlayerEnergy.modify_maxStamina(ReturnAttribute(AttributesEnum.max_stamina));
        PlayerEnergy.modify_energyRegen(ReturnAttribute(AttributesEnum.energy_regen));
        PlayerEnergy.modify_staminaRegen(ReturnAttribute(AttributesEnum.stamina_regen));
    }
    //////////////////////////////////////



    //////////////////////////////////////
    private (int, int)[] NonCombatSkillsStorage = new (int, int)[7]; //From Intrinsic //From other
    private (int, int)[] CombatSkillsStorage = new (int, int)[4]; //From Intrinsic //From other

    private void UpdateAllSkills()
    {
        for(int i = 0; i < 7; i++)
        {
            NonCombatSkillsStorage[i].Item1 = CoreStatsStorage[(int)STARTUP_DECLARATIONS.Skill_CoreTie[i]] * 2;
        }

        for (int i = 0; i < 4; i++)
        {
            CombatSkillsStorage[i].Item1 = CoreStatsStorage[(int)STARTUP_DECLARATIONS.CombatSkills_CoreTie[i]] * 2;
        }
    }

    public int ReturnDiminishingPoint(bool isCombat, int num)
    {
        if (isCombat)
        {
            return CoreStatsStorage[(int)STARTUP_DECLARATIONS.CombatSkills_CoreTie[num]] * 10;
        }
        else
        {
            return CoreStatsStorage[(int)STARTUP_DECLARATIONS.Skill_CoreTie[num]] * 10;
        }
    }

    public void ModifyNonCombatSkill(SkillsEnum SkillName, int value)
    {
        NonCombatSkillsStorage[(int)SkillName].Item2 += value;
    }

    public int ReturnNonCombatSkill(SkillsEnum SkillName)
    {
        return NonCombatSkillsStorage[(int)SkillName].Item1 + NonCombatSkillsStorage[(int)SkillName].Item2;
    }

    public void ModifyCombatSkill(CombatSkillsEnum SkillName, int value)
    {
        CombatSkillsStorage[(int)SkillName].Item2 += value;
        UpdateBaseAttributes();
    }

    public int ReturnCombatSkill(CombatSkillsEnum SkillName)
    {
        return CombatSkillsStorage[(int)SkillName].Item1 + CombatSkillsStorage[(int)SkillName].Item2;
    }
    //////////////////////////////////////


    private void Start()
    {
        Player = GameObject.Find("Player");
        abilitiesController = Player.GetComponentInChildren<AbilitiesController>();
        PlayerHealth = Player.GetComponentInChildren<Health>();
        PlayerEnergy = Player.GetComponent<Energy>();
        combatChecker = Player.GetComponentInChildren<CombatChecker>();
        uIController = GameObject.Find("UI").GetComponent<UIController>();
        eventQueue = GameObject.Find("EventDisplay").GetComponent<EventQueue>();

        EXPBarGhost = EXPUI.Find("EXPBarGhost").GetComponent<Image>();
        EXPBar = EXPUI.Find("EXPBar").GetComponent<Image>();

        for (int i = 0; i < STARTUP_DECLARATIONS.Number_of_Attributes; i++)
        {
            AttributesAdditiveEffects[i] = new List<(string, float)>();
            AttributesMultiplicativeEffects[i] = new List<(string, float)>();
        }
        level = 1;
        current_exp = 0;
        ImplantPoints = 0;
        EXPBar.fillAmount = 0;


        SetCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Strength, 5);
        SetCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Dexterity, 5);
        SetCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Aptitude, 5);
        SetCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Intelligence, 5);
        SetCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Toughness, 5);
        SetCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Willpower, 5);

        EXPUI.Find("CharacterLevel").Find("Text").GetComponent<TextMeshProUGUI>().text = "LEVEL " + level.ToString();
        /////////////////////////////////////
    }


    public float ReturnDamageMult(DamageSource damageSource)
    {
        return FinalAttributesStorage[(int)STARTUP_DECLARATIONS.DamageSource_AttributesTie[(int)damageSource]] / 100f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateDisplayXP();
    }
}
