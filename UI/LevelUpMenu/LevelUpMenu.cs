using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips;

    private Transform statsTooltip;
    private PlayerStats playerStats;
    private UIController uIController;

    private GameObject Larceny;
    private GameObject Science;
    private GameObject Medicine;
    private GameObject Engineering;
    private GameObject Speech;
    private GameObject Survival;
    private GameObject Perception;

    private GameObject Ranged;
    private GameObject Melee;
    private GameObject Armor;
    private GameObject Athletic;

    private GameObject Finalize;

    private TextMeshProUGUI GeneralPointsText;
    private TextMeshProUGUI CombatPointsText;
    private TextMeshProUGUI statNameText;
    private TextMeshProUGUI statDescText;

    private NonDiegeticController AudioControl;

    private Color originalColor;

    private int[] temp_generalSkills = new int[7];
    private int[] temp_combatSkills = new int[4];

    private int FreeGeneralSkillPoints;
    private int FreeCombatSkillPoints;

    private bool first_enable = true;

    void first_enable_func()
    {
        AudioControl = GameObject.Find("Non Diegetic Audio").GetComponent<NonDiegeticController>();
        Transform content = transform.Find("Content");

        statsTooltip = content.Find("StatsInfoTooltip");
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        uIController = GetComponentInParent<UIController>();

        Larceny = content.Find("FirstPanel").Find("GeneralSkillsLayout").Find("LarcenyPanel").Find("Var").gameObject;
        Science = content.Find("FirstPanel").Find("GeneralSkillsLayout").Find("SciencePanel").Find("Var").gameObject;
        Medicine = content.Find("FirstPanel").Find("GeneralSkillsLayout").Find("MedicinePanel").Find("Var").gameObject;
        Engineering = content.Find("FirstPanel").Find("GeneralSkillsLayout").Find("EngineeringPanel").Find("Var").gameObject;
        Speech = content.Find("FirstPanel").Find("GeneralSkillsLayout").Find("SpeechPanel").Find("Var").gameObject;
        Survival = content.Find("FirstPanel").Find("GeneralSkillsLayout").Find("SurvivalPanel").Find("Var").gameObject;
        Perception = content.Find("FirstPanel").Find("GeneralSkillsLayout").Find("PerceptionPanel").Find("Var").gameObject;

        Ranged = content.Find("FirstPanel").Find("CombatSkillsLayout").Find("RangedPanel").Find("Var").gameObject;
        Melee = content.Find("FirstPanel").Find("CombatSkillsLayout").Find("MeleePanel").Find("Var").gameObject;
        Armor = content.Find("FirstPanel").Find("CombatSkillsLayout").Find("ArmorPanel").Find("Var").gameObject;
        Athletic = content.Find("FirstPanel").Find("CombatSkillsLayout").Find("AthleticPanel").Find("Var").gameObject;

        Finalize = content.Find("FirstPanel").Find("FinalizeButton").gameObject;

        GeneralPointsText = content.Find("FirstPanel").Find("GeneralSkillsLayout").Find("Title").Find("SkillPoints").GetComponent<TextMeshProUGUI>();
        CombatPointsText = content.Find("FirstPanel").Find("CombatSkillsLayout").Find("Title").Find("SkillPoints").GetComponent<TextMeshProUGUI>();
        statNameText = statsTooltip.Find("Content").Find("StatName").GetComponent<TextMeshProUGUI>();
        statDescText = statsTooltip.Find("Content").Find("Description").GetComponent<TextMeshProUGUI>();

        originalColor = content.Find("FirstPanel").Find("GeneralSkillsLayout").Find("LarcenyPanel").Find("Var").GetComponent<TextMeshProUGUI>().color;
    }


    private void OnEnable()
    {
        if (first_enable)
        {
            first_enable = false;
            first_enable_func();
        }

        AudioControl.ChangeAudioSpecific(audioClips, true);

        for (int i = 0; i < 7; i++)
        {
            temp_generalSkills[i] = 0;
        }

        for (int i = 0; i < 4; i++)
        {
            temp_combatSkills[i] = 0;
        }

        DisableStatPanel(true);
        transform.Find("Content").Find("FirstPanel").Find("Header").Find("CharacterLevel").GetComponent<TextMeshProUGUI>().text = "LEVEL " + playerStats.returnLevel();
        Finalize.SetActive(false);
        FreeGeneralSkillPoints = playerStats.returnFreeSkillPoints();
        FreeCombatSkillPoints = playerStats.returnFreeSkillPoints();
    }

    public void IncreaseGeneralSkill(int skill)
    {
        Debug.Log(skill);
        int points = 1;
        int temp_skill = playerStats.ReturnNonCombatSkill((SkillsEnum)skill) + temp_generalSkills[skill];
        if (playerStats.ReturnDiminishingPoint(false, skill) <= temp_skill)
        {
            points = 2;
        }
        if (FreeGeneralSkillPoints >= points)
        {
            temp_generalSkills[skill] += 1;
            FreeGeneralSkillPoints -= points;
        }
    }
    public void DecreaseGeneralSkill(int skill)
    {
        int points = 1;
        int temp_skill = playerStats.ReturnNonCombatSkill((SkillsEnum)skill) + temp_generalSkills[skill];
        if (playerStats.ReturnDiminishingPoint(false, skill) <= temp_skill)
        {
            points = 2;
        }
        if (temp_generalSkills[skill] > 0)
        {
            temp_generalSkills[skill] -= 1;
            FreeGeneralSkillPoints += points;
        }
    }
    public void IncreaseCombatSkill(int skill)
    {
        int points = 1;
        int temp_skill = playerStats.ReturnCombatSkill((CombatSkillsEnum)skill) + temp_combatSkills[skill];
        if (playerStats.ReturnDiminishingPoint(true, skill) <= temp_skill)
        {
            points = 2;
        }
        if (FreeCombatSkillPoints > 0)
        {
            temp_combatSkills[skill] += 1;
            FreeCombatSkillPoints -= points;
        }
    }
    public void DecreaseCombatSkill(int skill)
    {
        int points = 1;
        int temp_skill = playerStats.ReturnCombatSkill((CombatSkillsEnum)skill) + temp_combatSkills[skill];
        if (playerStats.ReturnDiminishingPoint(true, skill) <= temp_skill)
        {
            points = 2;
        }
        if (temp_combatSkills[skill] > 0)
        {
            temp_combatSkills[skill] -= 1;
            FreeCombatSkillPoints += points;
        }
    }

    public void FinalizeFunc()
    {
        AudioControl.ChangeAudioGeneral();
        uIController.LevelUpMenuBool(false);
        for(int i = 0; i < 7; i++)
        {
            playerStats.ModifyNonCombatSkill((SkillsEnum)i, temp_generalSkills[i]);
        }

        for(int i = 0; i < 4; i++)
        {
            playerStats.ModifyCombatSkill((CombatSkillsEnum)i, temp_combatSkills[i]);
        }
    }

    public void EnableStatPanel(int statNum)
    {
        statsTooltip.gameObject.SetActive(true);
        statsTooltip.GetComponent<Animator>().Play("In");

        statNameText.text = STARTUP_DECLARATIONS.skillsDescriptions[statNum].Item1.ToUpper();
        statDescText.text = STARTUP_DECLARATIONS.skillsDescriptions[statNum].Item2;
    } 

    public void DisableStatPanel(bool startup = false)
    {
        if (!startup)
        {
            statsTooltip.gameObject.GetComponent<Animator>().Play("Out");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Larceny.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Larceny) + temp_generalSkills[0]).ToString();
        Science.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Science) + temp_generalSkills[1]).ToString();
        Medicine.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Medicine) + temp_generalSkills[2]).ToString();
        Engineering.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Engineering) + temp_generalSkills[3]).ToString();
        Speech.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Speech) + temp_generalSkills[4]).ToString();
        Survival.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Survival) + temp_generalSkills[5]).ToString();
        Perception.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Perception) + temp_generalSkills[6]).ToString();

        Ranged.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCombatSkill(CombatSkillsEnum.ranged_proficiency) + temp_combatSkills[0]).ToString();
        Melee.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCombatSkill(CombatSkillsEnum.melee_proficiency) + temp_combatSkills[1]).ToString();
        Armor.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCombatSkill(CombatSkillsEnum.armor_proficiency) + temp_combatSkills[2]).ToString();
        Athletic.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCombatSkill(CombatSkillsEnum.athletic_proficiency) + temp_combatSkills[3]).ToString();

        GeneralPointsText.text = FreeGeneralSkillPoints.ToString();
        CombatPointsText.text = FreeCombatSkillPoints.ToString();

        if(FreeGeneralSkillPoints + FreeCombatSkillPoints == 0)
        {
            Finalize.SetActive(true);
        }
        else
        {
            Finalize.SetActive(false);
        }

        CheckDiminishingPoints();
    }

    private void CheckDiminishingPoints()
    {
        Larceny.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(false, 0) <= playerStats.ReturnNonCombatSkill(SkillsEnum.Larceny) + temp_generalSkills[0] ? Color.red : originalColor);
        Science.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(false, 1) <= playerStats.ReturnNonCombatSkill(SkillsEnum.Science) + temp_generalSkills[1] ? Color.red : originalColor);
        Medicine.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(false, 2) <= playerStats.ReturnNonCombatSkill(SkillsEnum.Medicine) + temp_generalSkills[2] ? Color.red : originalColor);
        Engineering.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(false, 3) <= playerStats.ReturnNonCombatSkill(SkillsEnum.Engineering) + temp_generalSkills[3] ? Color.red : originalColor);
        Speech.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(false, 4) <= playerStats.ReturnNonCombatSkill(SkillsEnum.Speech) + temp_generalSkills[4] ? Color.red : originalColor);
        Survival.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(false, 5) <= playerStats.ReturnNonCombatSkill(SkillsEnum.Survival) + temp_generalSkills[5] ? Color.red : originalColor);
        Perception.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(false, 6) <= playerStats.ReturnNonCombatSkill(SkillsEnum.Perception) + temp_generalSkills[6] ? Color.red : originalColor);

        Ranged.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(true, 0) <= playerStats.ReturnCombatSkill(CombatSkillsEnum.ranged_proficiency) + temp_combatSkills[0] ? Color.red : originalColor);
        Melee.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(true, 1) <= playerStats.ReturnCombatSkill(CombatSkillsEnum.melee_proficiency) + temp_combatSkills[1] ? Color.red : originalColor);
        Armor.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(true, 2) <= playerStats.ReturnCombatSkill(CombatSkillsEnum.armor_proficiency) + temp_combatSkills[2] ? Color.red : originalColor);
        Athletic.GetComponent<TextMeshProUGUI>().color = (playerStats.ReturnDiminishingPoint(true, 3) <= playerStats.ReturnCombatSkill(CombatSkillsEnum.athletic_proficiency) + temp_combatSkills[3] ? Color.red : originalColor);
    }
}
