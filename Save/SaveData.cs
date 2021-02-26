using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SaveData : MonoBehaviour
{
    [SerializeField] private bool Initialize_Default = true;
    [SerializeField] private List<GameObject> GeneralInv = new List<GameObject>();
    [SerializeField] private GameObject[] EquippedWeapons = new GameObject[2];
    [SerializeField] private GameObject EquippedConsumable;
    [SerializeField] private GameObject[] EquippedArmor = new GameObject[3];
    [SerializeField] private GameObject[] SlottedAbilities_FROMHOLDER = new GameObject[4];

    [SerializeField] private Transform CurrentQuest;
    [SerializeField] private Transform CurrentObj;

    Transform player;
    Inventory inv;
    AbilitiesController AC;
    QuestsHolder QH;

    public void SaveAndExit()
    {
        Save();
        Debug.Log("Saving... ");
        Application.Quit();
    }

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        inv = player.GetComponentInChildren<Inventory>();
        AC = player.GetComponentInChildren<AbilitiesController>();
        QH = FindObjectOfType<QuestsHolder>();

        if (!Initialize_Default)
        {
            StartCoroutine("Load");
        }
    }

    private void Save()
    {
        (List<GameObject>, GameObject[], GameObject, GameObject[]) InvData = inv.DataTransfer();
        GeneralInv = InvData.Item1;
        EquippedWeapons = InvData.Item2;
        EquippedConsumable = InvData.Item3;
        EquippedArmor = InvData.Item4;
    }


    IEnumerator Load()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        foreach (GameObject gmeObj in GeneralInv) //General items
        {
            if (gmeObj)
            {
                inv.AddItem(Instantiate(gmeObj));
            }
        }

        GameObject temp;
        for(int i = 0; i < 2; ++i) //Weapons //Equip
        {
            if (EquippedWeapons[i])
            {
                temp = Instantiate(EquippedWeapons[i]);
                inv.AddItem(temp);
                inv.EquipWeapon(temp, i);
            }
        }

        if (EquippedConsumable)
        {
            temp = Instantiate(EquippedConsumable);//Consumables //Equip
            inv.AddItem(temp);
            inv.EquipConsumable(temp);
        }

        for (int i = 0; i < 3; ++i) //Armor //Equip
        {
            if (EquippedArmor[i])
            {
                temp = Instantiate(EquippedArmor[i]);
                inv.AddItem(temp);
                inv.EquipArmor(temp, (ArmorType)i);
            }
        }

        for (int i = 0; i < 4; ++i) //Abilities //Equip
        {
            if (SlottedAbilities_FROMHOLDER[i])
            {
                Ability abil = SlottedAbilities_FROMHOLDER[i].GetComponent<Ability>();
                AC.SlotAbility(abil, i);
            }
        }

        if (CurrentQuest)
        {
            QH.LoadQuest(CurrentQuest, CurrentObj);
        }
    }
}
