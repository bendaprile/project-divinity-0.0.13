using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityUIMenu : MonoBehaviour
{
    [SerializeField] private Sprite DefaultIcon = null;

    private AbilitiesController AC;
    private AbilityMenuController AMC;
    private Transform[] AbilitySlots = new Transform[4];

    // Start is called before the first frame update
    void Awake()
    {
        AC = GameObject.Find("Player").GetComponentInChildren<AbilitiesController>();
        AMC = GetComponentInParent<AbilityMenuController>();

        AbilitySlots[0] = transform.Find("Ability0");
        AbilitySlots[1] = transform.Find("Ability1");
        AbilitySlots[2] = transform.Find("Ability2");
        AbilitySlots[3] = transform.Find("Ability3");
    }

    public void UnslotAbility(int loc)
    {
        AC.SlotAbility(null, loc);
        AMC.UpdateAbilityPanel();
    }

    public void UnslotAbilityAnim(int loc)
    {
        if (AC.ReturnAbility(loc))
        {
            AbilitySlots[loc].Find("RemoveIcon").GetComponent<Animator>().Play("Remove Out");
        }
        AC.SlotAbility(null, loc);
        AMC.UpdateAbilityPanel();
    }

    public void HoverAbility(int loc)
    {
        if (AC.ReturnAbility(loc))
        {
            AbilitySlots[loc].Find("RemoveIcon").GetComponent<Animator>().Play("Remove In");
        }
    }

    public void LeaveHoverAbility(int loc)
    {
        if (AC.ReturnAbility(loc))
        {
            AbilitySlots[loc].Find("RemoveIcon").GetComponent<Animator>().Play("Remove Out");
        }
    }

    private void Update()
    {
        for(int i = 0; i < 4; ++i)
        {
            AbilitySlots[i].Find("Icon").GetComponent<Image>().sprite =
            (AC.ReturnAbility(i) ? AC.ReturnAbility(i).ReturnBasicStats().Item2 : DefaultIcon);

            AbilitySlots[i].Find("Icon").GetComponent<Image>().color = AC.ReturnAbility(i) ? Color.white : Color.clear;
        }
    }
}
