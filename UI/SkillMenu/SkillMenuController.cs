using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillMenuController : MonoBehaviour
{
    [SerializeField] private Transform Name_ValuePrefab;

    private PlayerStats playerStats;

    private GameObject Strength;
    private GameObject Dexterity;
    private GameObject Aptitude;
    private GameObject Intelligence;
    private GameObject Toughness;
    private GameObject Willpower;

    private GameObject Vigor;
    private GameObject Cerebral;
    private GameObject Fortitude;

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

    private Transform ImplantStatsTooltip;
    private Transform StatInfoTooltip;

    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        Transform content = transform.Find("Content");
        Strength = content.Find("IntrinsicSkillLayout").Find("StrengthPanel").Find("StatValue").gameObject;
        Dexterity = content.Find("IntrinsicSkillLayout").Find("DexterityPanel").Find("StatValue").gameObject;
        Aptitude = content.Find("IntrinsicSkillLayout").Find("AptitudePanel").Find("StatValue").gameObject;
        Intelligence = content.Find("IntrinsicSkillLayout").Find("IntelligencePanel").Find("StatValue").gameObject;
        Toughness = content.Find("IntrinsicSkillLayout").Find("ToughnessPanel").Find("StatValue").gameObject;
        Willpower = content.Find("IntrinsicSkillLayout").Find("WillpowerPanel").Find("StatValue").gameObject;
        Vigor = content.Find("IntrinsicSkillLayout").Find("VigorPanel").Find("StatValue").gameObject;
        Cerebral = content.Find("IntrinsicSkillLayout").Find("CerebralPanel").Find("StatValue").gameObject;
        Fortitude = content.Find("IntrinsicSkillLayout").Find("FortitudePanel").Find("StatValue").gameObject;

        Larceny = content.Find("GeneralSkillsLayout").Find("LarcenyPanel").Find("StatValue").gameObject;
        Science = content.Find("GeneralSkillsLayout").Find("SciencePanel").Find("StatValue").gameObject;
        Medicine = content.Find("GeneralSkillsLayout").Find("MedicinePanel").Find("StatValue").gameObject;
        Engineering = content.Find("GeneralSkillsLayout").Find("EngineeringPanel").Find("StatValue").gameObject;
        Speech = content.Find("GeneralSkillsLayout").Find("SpeechPanel").Find("StatValue").gameObject;
        Survival = content.Find("GeneralSkillsLayout").Find("SurvivalPanel").Find("StatValue").gameObject;
        Perception = content.Find("GeneralSkillsLayout").Find("PerceptionPanel").Find("StatValue").gameObject;

        Ranged = content.Find("CombatSkillsLayout").Find("RangedPanel").Find("StatValue").gameObject;
        Melee = content.Find("CombatSkillsLayout").Find("MeleePanel").Find("StatValue").gameObject;
        Armor = content.Find("CombatSkillsLayout").Find("ArmorPanel").Find("StatValue").gameObject;
        Athletic = content.Find("CombatSkillsLayout").Find("AthleticPanel").Find("StatValue").gameObject;

        ImplantStatsTooltip = content.Find("ImplantStatsTooltip");
        StatInfoTooltip = content.Find("StatsInfoTooltip");

        DisableImplantStatPanel(true);
        DisableStatPanel(true);
    }

    // Update is called once per frame
    void Update()
    {
        Strength.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Strength)).ToString();
        Dexterity.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Dexterity)).ToString();
        Aptitude.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Aptitude)).ToString();
        Intelligence.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Intelligence)).ToString();
        Toughness.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Toughness)).ToString();
        Willpower.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCoreIntrinsicSkill(CoreIntrinsicSkillsEnum.Willpower)).ToString();

        Vigor.GetComponent<TextMeshProUGUI>().text = playerStats.ReturnDerivedIntrinsicSkill(DerivedIntrinsicSkillsEnum.Vigor).ToString();
        Cerebral.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnDerivedIntrinsicSkill(DerivedIntrinsicSkillsEnum.Cerebral)).ToString();
        Fortitude.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnDerivedIntrinsicSkill(DerivedIntrinsicSkillsEnum.Fortitude)).ToString();

        Larceny.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Larceny)).ToString();
        Science.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Science)).ToString();
        Medicine.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Medicine)).ToString();
        Engineering.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Engineering)).ToString();
        Speech.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Speech)).ToString();
        Survival.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Survival)).ToString();
        Perception.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnNonCombatSkill(SkillsEnum.Perception)).ToString();

        Ranged.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCombatSkill(CombatSkillsEnum.ranged_proficiency)).ToString();
        Melee.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCombatSkill(CombatSkillsEnum.melee_proficiency)).ToString();
        Armor.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCombatSkill(CombatSkillsEnum.armor_proficiency)).ToString();
        Athletic.GetComponent<TextMeshProUGUI>().text = (playerStats.ReturnCombatSkill(CombatSkillsEnum.athletic_proficiency)).ToString();
    }

    public void EnableStatPanel(int statNum, float posY)
    {
        DisableStatPanel();

        StatInfoTooltip.position = new Vector3(StatInfoTooltip.position.x, posY, StatInfoTooltip.position.z);

        if (StatInfoTooltip.localPosition.y < -230f)
        {
            StatInfoTooltip.localPosition = new Vector3(StatInfoTooltip.localPosition.x, -230f, StatInfoTooltip.localPosition.z);
        }
        else if (StatInfoTooltip.localPosition.y > 130f)
        {
            StatInfoTooltip.localPosition = new Vector3(StatInfoTooltip.localPosition.x, 130f, StatInfoTooltip.localPosition.z);
        }

        StatInfoTooltip.GetComponent<Animator>().Play("In");

        StatInfoTooltip.Find("Content").Find("StatName").GetComponent<TextMeshProUGUI>().text = STARTUP_DECLARATIONS.skillsDescriptions[statNum].Item1.ToUpper();
        StatInfoTooltip.Find("Content").Find("Description").GetComponent<TextMeshProUGUI>().text = STARTUP_DECLARATIONS.skillsDescriptions[statNum].Item2;
    }

    public void EnableImplantStatPanel(string itemName, string description, ItemQuality itemClass, List<(string, string)> data, float posY)
    {
        DisableImplantStatPanel();

        ImplantStatsTooltip.position = new Vector3(ImplantStatsTooltip.position.x, posY, ImplantStatsTooltip.position.z);

        if (ImplantStatsTooltip.localPosition.y < -230f)
        {
            ImplantStatsTooltip.localPosition = new Vector3(ImplantStatsTooltip.localPosition.x, -230f, ImplantStatsTooltip.localPosition.z);
        }
        else if (ImplantStatsTooltip.localPosition.y > 130f)
        {
            ImplantStatsTooltip.localPosition = new Vector3(ImplantStatsTooltip.localPosition.x, 130f, ImplantStatsTooltip.localPosition.z);
        }

        ImplantStatsTooltip.GetComponent<Animator>().Play("In");
        ImplantStatsTooltip.Find("Content").Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemName.ToUpper();
        ImplantStatsTooltip.Find("Content").Find("Border").GetComponent<Image>().color = STARTUP_DECLARATIONS.itemQualityColors[itemClass];
        ImplantStatsTooltip.Find("Content").Find("Description").GetComponent<TextMeshProUGUI>().text = description;

        foreach ((string, string) item in data)
        {
            Transform temp = Instantiate(Name_ValuePrefab, ImplantStatsTooltip.Find("Content").Find("Stats"));
            temp.Find("Name").GetComponent<TextMeshProUGUI>().text = item.Item1;
            temp.Find("Value").GetComponent<TextMeshProUGUI>().text = item.Item2;
        }
    }

    public void DisableStatPanel(bool startup = false)
    {
        if (!startup)
        {
            StatInfoTooltip.gameObject.GetComponent<Animator>().Play("Out");
        }
    }

    public void DisableImplantStatPanel(bool startup = false)
    {
        foreach (Transform child in ImplantStatsTooltip.Find("Content").Find("Stats"))
        {
            Destroy(child.gameObject);
        }

        if (!startup)
        {
            ImplantStatsTooltip.gameObject.GetComponent<Animator>().Play("Out");
        }
    }
}
