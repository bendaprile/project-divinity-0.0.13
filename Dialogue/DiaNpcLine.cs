using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaNpcLine : DiaMaster
{
    [SerializeField] private GameObject NPC_if_not_self = null;

    [SerializeField] [TextArea(3, 10)] private List<string> Lines = new List<string>();

    protected enum DiaAnimation {
        Idle,
        No,
        Yes,
        Shrug,
        Angry,
        Begging,
        Sad,
        Pouting,
        Talking,
        Annoyed,
        Thoughtful,
        Acknowledge,
        NeckStretch,
        Relieved,
        Sarcastic,
    };

    public enum TransitionTime
    {
        trans_200ms,
        trans_350ms,
        trans_450ms,
        trans_1s,
    }

    [SerializeField] protected DiaAnimation DiaAnima = DiaAnimation.Idle;

    [SerializeField] private bool cycleLines = false;

    [SerializeField] private bool cycleLinesNoWrapAround = false;
    [SerializeField] private bool resetCycle = false;

    [SerializeField] private Transform NewStartingLine;

    [SerializeField] private Transform Dest = null;

    private int currentLine = 0;

    public Transform Return_NewStartingLine()
    {
        return NewStartingLine;
    }

    public string return_line()
    {
        QuestsHolder QH = GameObject.Find("QuestsHolder").GetComponent<QuestsHolder>();

        Gesture();

        //Camera + Name
        List<Transform> targetTransforms = new List<Transform>();
        targetTransforms.Add(GameObject.Find("Player").transform);
        if (NPC_if_not_self)
        {
            FindObjectOfType<DiaParent>().Set_NPC_name(NPC_if_not_self.GetComponent<HumanoidMaster>().Return_Name());
            targetTransforms.Add(NPC_if_not_self.transform);
        }
        else
        {
            FindObjectOfType<DiaParent>().Set_NPC_name(GetComponentInParent<DiaRoot>().ReturnNPC_name());
            targetTransforms.Add(transform.parent);
        }

        CameraStateController camStateController = FindObjectOfType<CameraStateController>();
        camStateController.SetDiaTargetGroup(targetTransforms);
        //Camera + Name


        if (QuestTaskReturn)
        {
            QH.CheckTaskCompletion(QuestTaskReturn, quest_num);
            GetComponentInParent<DiaRoot>().Modify_diaCount(false);
        }

        if (Quest)
        {
            QH.AddQuest(Quest);
            Quest = null;
        }


        if (cycleLines)
        {
            string templine = Lines[currentLine];
            currentLine = (currentLine + 1) % Lines.Count;
            return templine;
        }
        else if (cycleLinesNoWrapAround)
        {
            string templine = Lines[currentLine];
            if(currentLine < (Lines.Count - 1))
            {
                currentLine += 1;
            }
            return templine;
        }
        else if(Lines.Count > 0)
        {
            return Lines[0];
        }
        else
        {
            return "";
        }
    }

    private void Gesture()
    {
        EnemyAnimationUpdater EAU;
        if (NPC_if_not_self)
        {
            EAU = NPC_if_not_self.GetComponent<HumanoidMaster>().Return_Animation_Updater();
        }
        else
        {
            EAU = GetComponentInParent<HumanoidMaster>().Return_Animation_Updater();
        }

        if (EAU)
        {
            switch (DiaAnima)
            {
                case DiaAnimation.Idle:
                    EAU.PlayAnimation("idle", unScaledTime: true);
                    break;
                case DiaAnimation.No:
                    EAU.PlayGesture(0, true);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.Yes:
                    EAU.PlayGesture(1, true);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.Shrug:
                    EAU.PlayGesture(2);
                    EAU.SetTransitionTrigger(TransitionTime.trans_200ms);
                    break;
                case DiaAnimation.Angry:
                    EAU.PlayGesture(3);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.Begging:
                    EAU.PlayGesture(4);
                    EAU.SetTransitionTrigger(TransitionTime.trans_1s);
                    break;
                case DiaAnimation.Sad:
                    EAU.PlayGesture(5);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.Pouting:
                    EAU.PlayGesture(6);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.Talking:
                    EAU.PlayGesture(7);
                    EAU.SetTransitionTrigger(TransitionTime.trans_450ms);
                    break;
                case DiaAnimation.Annoyed:
                    EAU.PlayGesture(8, true);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.Thoughtful:
                    EAU.PlayGesture(9, true);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.Acknowledge:
                    EAU.PlayGesture(10);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.NeckStretch:
                    EAU.PlayGesture(11);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.Relieved:
                    EAU.PlayGesture(12);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
                case DiaAnimation.Sarcastic:
                    EAU.PlayGesture(13);
                    EAU.SetTransitionTrigger(TransitionTime.trans_350ms);
                    break;
            }
        }
    }

    public Transform return_dest()
    {
        return Dest;
    }

    public void Reset()
    {
        if (resetCycle)
        {
            currentLine = 0;
        }
    }
}
