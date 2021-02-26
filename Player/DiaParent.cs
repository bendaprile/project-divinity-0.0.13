using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaParent : MonoBehaviour
{
    private TextMeshProUGUI npcText;

    [SerializeField] private GameObject PlayerLinePrefab = null;
    [SerializeField] private UIController uIcontroller = null;
    [SerializeField] private GameObject NpcTextPanel = null;
    [SerializeField] private GameObject ContinueButton = null; 
    [SerializeField] private Transform DiaPlayerPanel = null;
    [SerializeField] private Transform DiaNoOptions = null;
    [SerializeField] private Transform BottomSpacerPrefab = null;
    [SerializeField] private TextMeshProUGUI NPC_name = null;

    bool first_setup = true;
    Transform next_dest; //used when there is no player input
    private DiaRoot diaRoot;

    public void Set_NPC_name(string name)
    {
        NPC_name.text = name;
    }


    public void SetupDia(Transform DiaRoot)
    {
        if (first_setup)
        {
            FirstSetup();
        }

        diaRoot = DiaRoot.GetComponent<DiaRoot>();

        next_dest = null; //used when there is no player input
        ViewText(diaRoot.ReturnStarting());
    }


    public void Continue(Transform dest)
    {
        if ((dest == DiaNoOptions && next_dest == null ) || dest == null)
        {
            ResetNpcLines();
            uIcontroller.DialogueMenuBool();
        }
        else if(dest == DiaNoOptions) //next line // this object is a child of the continue button, but it doesnt matter where it is. A button cannot send null, so I have this random transform
        {
            ViewText(next_dest);
        }
        else //next line
        {
            ViewText(dest);
        }
    }


    private void ViewText(Transform selectedLine)
    {
        foreach(Transform child in DiaPlayerPanel) //clean player panel
        {
            Destroy(child.gameObject);
        }

        if (selectedLine.GetComponent<DiaNpcLine>().Return_NewStartingLine()) //Before return_line so that the quest new startingLine (in return_line) overrides this
        { //This only matters in a wierd case where the quest is instantly completed once you start talking (like a spawner with 0 enemies for a spawnerkill quest)
            diaRoot.ModifyStarting(selectedLine.GetComponent<DiaNpcLine>().Return_NewStartingLine());
        }
        npcText.text = selectedLine.GetComponent<DiaNpcLine>().return_line();
        next_dest = selectedLine.GetComponent<DiaNpcLine>().return_dest();

        bool is_playerLines = SetupPlayerLines(selectedLine);

        if (is_playerLines)
        {
            ContinueButton.SetActive(false);
            DiaPlayerPanel.gameObject.SetActive(true);
        }
        else
        {
            ContinueButton.SetActive(true);
            DiaPlayerPanel.gameObject.SetActive(false);
        }
    }

    private bool SetupPlayerLines(Transform selectedLine)
    {
        bool is_playerLines = false;
        foreach (Transform child in selectedLine)
        {
            if (child.gameObject.activeSelf)
            {
                DiaPlayerLine TempChild = child.GetComponent<DiaPlayerLine>();
                is_playerLines = true;
                GameObject temp = Instantiate(PlayerLinePrefab, DiaPlayerPanel);
                temp.GetComponent<DiaPlayerUIPrefab>().Setup(TempChild);
            }
        }
        Instantiate(BottomSpacerPrefab, DiaPlayerPanel);

        return is_playerLines;
    }

    private void ResetNpcLines()
    {
        DiaNpcLine[] npcLines = diaRoot.GetComponentsInChildren<DiaNpcLine>();

        foreach (DiaNpcLine line in npcLines)
        {
            line.Reset();
        }
    }

    private void FirstSetup()
    {
        npcText = NpcTextPanel.GetComponent<TextMeshProUGUI>();
        first_setup = false;
    }
}
