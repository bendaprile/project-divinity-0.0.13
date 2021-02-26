using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaPlayerUIPrefab : MonoBehaviour
{
    DiaParent diaParent;
    QuestsHolder QH;

    bool pressable;

    DiaPlayerLine TempChildStorage;



    public void Setup(DiaPlayerLine TempChild)
    {
        TempChildStorage = TempChild;
        QH = GameObject.Find("QuestsHolder").GetComponent<QuestsHolder>();
        TextMeshProUGUI textRef = transform.GetComponent<TextMeshProUGUI>();
        pressable = TempChild.Check_Accessible(textRef);

        diaParent = GameObject.Find("DialogueMenu").GetComponent<DiaParent>();
    }


    public void ButtonPressed()
    {
        if (pressable)
        {
            TempChildStorage.Pressed(); //This has the logic in the line instead out here like the others

            if (TempChildStorage.return_quest() != null)
            {
                QH.AddQuest(TempChildStorage.return_quest());
            }

            if(TempChildStorage.return_new_start() != null)
            {
                TempChildStorage.GetComponentInParent<DiaRoot>().ModifyStarting(TempChildStorage.return_new_start());
            }

            if (TempChildStorage.QuestTaskReturn)
            {
                QH.CheckTaskCompletion(TempChildStorage.QuestTaskReturn, TempChildStorage.quest_num);
                TempChildStorage.GetComponentInParent<DiaRoot>().Modify_diaCount(false);
            }

            diaParent.Continue(TempChildStorage.return_dest());
        }
    }

}
