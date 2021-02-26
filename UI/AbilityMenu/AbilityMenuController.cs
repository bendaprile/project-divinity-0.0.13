using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityMenuController : MonoBehaviour
{
    [SerializeField] private AbilityUIPrefab AuiP = null;
    [SerializeField] private GameObject Name_ValuePrefab = null;
    [SerializeField] private Transform AbilityPanelContent = null;
    [SerializeField] private Animator lightningAnimator = null;
    [SerializeField] private Animator spacetimeAnimator = null;
    [SerializeField] private Animator exoskeletonAnimator = null;
    [SerializeField] private Animator regenAnimator = null;
    [SerializeField] private Animator factionAnimator = null;

    PlayerAbilityUIMenu PAuiM;
    private AbilityType lastAbilityType = AbilityType.Lightning;
    private Transform AbilityHolder;
    private Transform AbilityTooltip;
    private Transform AbilityNavMenu;
    private bool First_run = true;


    private void first_run_func()
    {
        PAuiM = transform.GetComponentInChildren<PlayerAbilityUIMenu>();
        AbilityHolder = GameObject.Find("AbilityHolder").transform;
        AbilityTooltip = transform.Find("Content").Find("AbilityTooltip");
        AbilityNavMenu = transform.Find("Content").Find("AbilityNavMenu").Find("ButtonList");
        StartCoroutine(ExecuteAfterTime(0.01f));
        First_run = false;
    }

    private void OnEnable()
    {
        if (First_run)
        {
            first_run_func();
        }

        MassDisable(true);
        LockOutCategories();

        if (!First_run)
        {
            UpdateAbilityPanel();
        }
    }


    private void LockOutCategories()
    {
        Transform[] abilityGroups = new Transform[5]{ AbilityHolder.Find("Lightning"), AbilityHolder.Find("Spacetime"), AbilityHolder.Find("Exoskeleton"), AbilityHolder.Find("Regeneration"), AbilityHolder.Find("Faction") };
        Transform[] abilityButtons = new Transform[5] { AbilityNavMenu.Find("LightningButton"), AbilityNavMenu.Find("SpacetimeButton"), AbilityNavMenu.Find("ExoskeletonButton"), AbilityNavMenu.Find("RegenerationButton"), AbilityNavMenu.Find("FactionButton") };
        string[] properText = new string[5] { "LIGHTNING", "SPACETIME", "EXOSKELETON", "REGEN", "FACTION" };

        
        for(int i = 0; i < abilityGroups.Length; ++i)
        {
            Transform A = abilityGroups[i];
            bool one_ready = false;
            foreach(Transform child in A)
            {
                if (child.GetComponent<Ability>().ReturnAccessible())
                {
                    one_ready = true;
                }
            }
            abilityButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = one_ready ? properText[i] : "UNKNOWN";
        }
    }


    public void AbilityEnable(int type)
    {
        if ((AbilityType)type == lastAbilityType) { return; }

        MassDisable();
        lastAbilityType = (AbilityType)type;
        UpdateAbilityPanel();
    }


    private void MassDisable(bool resetAll = false)
    {
        ResetButtons(resetAll);
        DisableStatPanel(true);
    }

    private void ResetButtons(bool resetAll)
    {
        if (resetAll)        {            lightningAnimator.Play("Normal");            spacetimeAnimator.Play("Normal");            exoskeletonAnimator.Play("Normal");            regenAnimator.Play("Normal");            factionAnimator.Play("Normal");            return;        }        switch (lastAbilityType)        {            case AbilityType.Lightning:                lightningAnimator.Play("Pressed to Normal");                break;            case AbilityType.Spacetime:                spacetimeAnimator.Play("Pressed to Normal");                break;            case AbilityType.Exoskeleton:                exoskeletonAnimator.Play("Pressed to Normal");                break;            case AbilityType.Regeneration:                regenAnimator.Play("Pressed to Normal");                break;            case AbilityType.Faction:                factionAnimator.Play("Pressed to Normal");                break;        }
    }

    public void EnableStatPanel(string abilityName, string description, List<(string, string)> data, float posY)
    {        DisableStatPanel();        AbilityTooltip.gameObject.SetActive(true);        AbilityTooltip.GetComponent<Animator>().Play("In");        AbilityTooltip.position = new Vector3(AbilityTooltip.position.x, posY, AbilityTooltip.position.z);        if (AbilityTooltip.localPosition.y > 257f)
        {
            AbilityTooltip.localPosition = new Vector3(AbilityTooltip.localPosition.x, 257f, AbilityTooltip.localPosition.z);
        }
        else if (AbilityTooltip.localPosition.y < -180f)
        {
            AbilityTooltip.localPosition = new Vector3(AbilityTooltip.localPosition.x, -180f, AbilityTooltip.localPosition.z);
        }        AbilityTooltip.Find("Content").Find("AbilityName").GetComponent<TextMeshProUGUI>().text = abilityName.ToUpper();
        AbilityTooltip.Find("Content").Find("Description").GetComponent<TextMeshProUGUI>().text = description;

        foreach ((string, string) item in data)        {            GameObject temp = Instantiate(Name_ValuePrefab, AbilityTooltip.Find("Content").Find("Stats"));            temp.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.Item1;            temp.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = item.Item2;        }
    }


    public void DisableStatPanel(bool startup = false)
    {
        foreach (Transform child in AbilityTooltip.Find("Content").Find("Stats"))
        {
            Destroy(child.gameObject);
        }

        if (!startup && AbilityTooltip.gameObject.activeInHierarchy)        {            AbilityTooltip.gameObject.GetComponent<Animator>().Play("Out");        }
    }

    public void UpdateAbilityPanel()
    {
        DestoryAbilityPanel();
        PopulateAbilityPanel(lastAbilityType);
    }


    private void DestoryAbilityPanel()
    {
        foreach (Transform item in AbilityPanelContent)
        {
            Destroy(item.gameObject);
        }
    }

    private void PopulateAbilityPanel(AbilityType type)
    {
        Transform abilityGroup = null;
        switch (type)
        {
            case AbilityType.Lightning:
                lightningAnimator.Play("Hover to Pressed");
                abilityGroup = AbilityHolder.Find("Lightning");
                break;
            case AbilityType.Spacetime:
                spacetimeAnimator.Play("Hover to Pressed");
                abilityGroup = AbilityHolder.Find("Spacetime");
                break;
            case AbilityType.Exoskeleton:
                exoskeletonAnimator.Play("Hover to Pressed");
                abilityGroup = AbilityHolder.Find("Exoskeleton");
                break;
            case AbilityType.Regeneration:
                regenAnimator.Play("Hover to Pressed");
                abilityGroup = AbilityHolder.Find("Regeneration");
                break;
            case AbilityType.Faction:
                factionAnimator.Play("Hover to Pressed");
                abilityGroup = AbilityHolder.Find("Faction");
                break;
        }

        foreach (Transform A in abilityGroup)
        {
            Ability abilityVar = A.GetComponent<Ability>();
            AbilityUIPrefab temp = Instantiate(AuiP, AbilityPanelContent);
            temp.Setup(abilityVar);
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        AbilityNavMenu.gameObject.SetActive(false);
        AbilityNavMenu.gameObject.SetActive(true);
        UpdateAbilityPanel();
    }
}
