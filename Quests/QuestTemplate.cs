using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class QuestTemplate : MonoBehaviour
{
    [SerializeField] private GameObject Quest_Fail_Flag;
    [SerializeField] private GameObject Quest_Set_Flag;

    public string QuestName; //MUST BE UNIQUE
    public int xp_reward;
    [TextArea(3, 10)] public string questDescription = "";
    public int suggestedLevel = 1;
    public QuestCategory questCategory = QuestCategory.Miscellaneous;

    private List<GameObject> HiddenObjectiveList = new List<GameObject>();
    private GameObject ActiveObjective;
    private List<GameObject> CompletedObjectiveList = new List<GameObject>();

    private EventQueue eventQueue;
    private QuestsHolder questsHolder;
    private QuestHUD QHud;
    public Zone_Flags ZF;

    public QuestObjective returnActiveObjective()
    {
        return ActiveObjective.GetComponent<QuestObjective>();
    }

    public List<QuestObjective> returnCompletedObjectives()
    {
        List<QuestObjective> temp = new List<QuestObjective>();
        foreach(GameObject iter in CompletedObjectiveList)
        {
            temp.Add(iter.GetComponent<QuestObjective>());
        }
        return temp;
    }

    public (bool, List<(Vector2, float)>) returnActiveLocs()
    {
        return ActiveObjective.GetComponent<QuestObjective>().ReturnLocs();
    }

    private void Awake()
    {
        eventQueue = GameObject.Find("EventDisplay").GetComponent<EventQueue>();
        questsHolder = GameObject.Find("QuestsHolder").GetComponent<QuestsHolder>();
        QHud = GameObject.Find("QuestDisplay").GetComponent<QuestHUD>();
        ZF = FindObjectOfType<Zone_Flags>();

        if (QuestName.Length > 27) { Debug.LogError("Either change QuestUI or Length of this QuestName. This will not look good in UI");  }
    }

    public void QuestStart(Transform StartingObj = null) //StartingObj is for Debug and Saving for now
    {
        bool first = true;
        foreach(Transform child in transform)
        {
            if (first)
            {
                if (StartingObj && StartingObj != child)
                {
                    //CompletedObjectiveList.Add(child.gameObject);
                }
                else
                {
                    ActiveObjective = child.gameObject;
                    first = false;
                }
            }
            else
            {
                HiddenObjectiveList.Add(child.gameObject);
            }
        }
        FlagCheck();
        ActiveObjective.GetComponent<QuestObjective>().initialize(); //Has to be after hidden Obj are setup or it can instantly finish (THIS WAS A REALLY ANNOYING BUG REMEMBER THIS)
    }

    public void FlagCheck()
    {
        if (Quest_Fail_Flag && ZF.CheckFlag(Quest_Fail_Flag.name)) //Entire quest failed
        {
            questsHolder.FullQuestCompleted(gameObject, true);
        }
    }

    public void ObjectiveFinished()
    {
        CompletedObjectiveList.Add(ActiveObjective);

        /////
        EventData tempEvent = new EventData();
        tempEvent.Setup(EventTypeEnum.QuestObjCompleted, GetComponentInParent<QuestTemplate>().QuestName);
        eventQueue.AddEvent(tempEvent);
        /////

        if (HiddenObjectiveList.Count > 0)
        {
            ActiveObjective = HiddenObjectiveList[0];
            HiddenObjectiveList.RemoveAt(0);
            ActiveObjective.GetComponent<QuestObjective>().initialize();

            /////////
            QHud.QuestDisplay(this);
            ////////
        }
        else
        {
            ActiveObjective = null;
            questsHolder.FullQuestCompleted(gameObject);
            ZF.SetFlag(Quest_Set_Flag.name); //Has to be after
        }
    }

    ///////////////////////ScriptedEventsOnly/////////////////
    public bool CompareObj(GameObject obj) //TODO Switch with a local flag holder
    {
        return (obj == ActiveObjective);
    }
}
