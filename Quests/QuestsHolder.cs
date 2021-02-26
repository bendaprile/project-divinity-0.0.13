using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsHolder : MonoBehaviour
{
    private List<GameObject> CompletedQuests = new List<GameObject>();
    private List<GameObject> ActiveQuests = new List<GameObject>();
    private GameObject FocusedQuestReference; //INCLUSIVE WITH ACTIVE QUEST

    private EventQueue eventQueue;
    private PlayerStats playerStats;
    private EventData tempEvent;
    private mapScript Map;
    private QuestHUD QHud;


    void Start()
    {
        tempEvent = new EventData();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        eventQueue = GameObject.Find("EventDisplay").GetComponent<EventQueue>();
        QHud = GameObject.Find("QuestDisplay").GetComponent<QuestHUD>();
    }

    public void AddQuest(Transform Quest)
    {
        QuestTemplate tempQuest = Quest.GetComponent<QuestTemplate>();
        tempEvent.Setup(EventTypeEnum.QuestStarted, tempQuest.QuestName);
        eventQueue.AddEvent(tempEvent);
        Quest.parent = transform;
        ActiveQuests.Add(Quest.gameObject);
        tempQuest.QuestStart();

        if (FocusedQuestReference == null)
        {
            QuestSetFocus(Quest.gameObject);
        }
    }

    public void LoadQuest(Transform Quest, Transform Obj) //LOADING ONLY
    {
        QuestTemplate tempQuest = Quest.GetComponent<QuestTemplate>();
        ActiveQuests.Add(Quest.gameObject);
        Quest.parent = transform;
        tempQuest.QuestStart(Obj);

        if (FocusedQuestReference == null)
        {
            QuestSetFocus(Quest.gameObject);
        }
    }

    public List<GameObject> ReturnActiveQuests()
    {
        return ActiveQuests;
    }

    public List<GameObject> ReturnCompletedQuests()
    {
        return CompletedQuests;
    }

    public void QuestSetFocus(GameObject quest_in)
    {
        if (!Map) //If there is not a reference for map already get it (the problem is I cannot get this reference if map is closed)
        {
            GameObject MapTemp = GameObject.Find("MapCanvasHolder");
            if (MapTemp)
            {
                Map = MapTemp.GetComponent<mapScript>();
            }   
        }

        if(quest_in == FocusedQuestReference)
        {
            FocusedQuestReference = null;
            QHud.DisableDisplay();
        }
        else
        {
            FocusedQuestReference = quest_in;
            QuestTemplate FocusedTemp = FocusedQuestReference.GetComponent<QuestTemplate>();
            QHud.QuestDisplay(FocusedTemp);
        }


        if (Map)
        {
            Map.UpdateObjLoc();
        }
    }

    public GameObject ReturnFocus()
    {
        return FocusedQuestReference;
    }

    public void FullQuestCompleted(GameObject UniqueObject, bool failed = false)
    {
        int temp_loc = 0;
        foreach(GameObject iter in ActiveQuests)
        {
            if (iter == UniqueObject)
            {
                QuestTemplate QuestiterScript = UniqueObject.GetComponent<QuestTemplate>();
                CompletedQuests.Add(iter);
                ActiveQuests.Remove(iter);
                QuestiterScript.questCategory = QuestCategory.Completed;

                if (FocusedQuestReference == UniqueObject)
                {
                    FocusedQuestReference = null;
                    QHud.DisableDisplay();
                }

                if (failed)
                {
                    tempEvent.Setup(EventTypeEnum.QuestFailed, QuestiterScript.QuestName);
                    eventQueue.AddEvent(tempEvent);
                }
                else
                {
                    playerStats.AddEXP(QuestiterScript.xp_reward);
                    tempEvent.Setup(EventTypeEnum.QuestCompleted, QuestiterScript.QuestName);
                    eventQueue.AddEvent(tempEvent);
                }
    
                break;
            }
            temp_loc += 1;
        }
    }

    public void CheckFlags(string Flag)
    {
        for (int i = ActiveQuests.Count - 1; i >= 0; i--) //A quest could be completed
        {
            QuestTemplate TempActiveObj = ActiveQuests[i].GetComponent<QuestTemplate>();
            TempActiveObj.FlagCheck(); //Check the Template too
            TempActiveObj.returnActiveObjective().FlagCheckObj();
        }
    }

    public void CheckFetchObjectives(GameObject item)
    {
        for(int i = ActiveQuests.Count - 1; i >= 0; i--) //A quest could be completed
        {
            QuestObjective TempActiveObj = ActiveQuests[i].GetComponent<QuestTemplate>().returnActiveObjective();
            if (TempActiveObj.ReturnType() == ObjectiveType.Fetch)
            {
                ((FetchObjective)TempActiveObj).UpdateItemCount(item);
            }
        }
    }

    public void CheckGenericKillObjectives(GameObject enemy, Vector2 enemyLoc)
    {
        EnemyTemplateMaster enemyStats = enemy.GetComponent<EnemyTemplateMaster>();
        for (int i = ActiveQuests.Count - 1; i >= 0; i--) //A quest could be completed
        {
            QuestObjective TempActiveObj = ActiveQuests[i].GetComponent<QuestTemplate>().returnActiveObjective();
            if (TempActiveObj.ReturnType() == ObjectiveType.GenericKill)
            {
                ((GenericKillObjective)TempActiveObj).UpdateKillCounts(enemyStats, enemyLoc);
            }
        }
    }

    public void CheckTaskCompletion(GameObject ObjectiveRef, int num) //Many types of quests
    {
        for (int i = ActiveQuests.Count - 1; i >= 0; i--) //A quest could be completed
        {
            QuestObjective TempActiveObj = ActiveQuests[i].GetComponent<QuestTemplate>().returnActiveObjective();
            TempActiveObj.TaskCompletion(ObjectiveRef, num);
        }
    }
}
