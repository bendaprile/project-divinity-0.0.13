using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WorldMenuController : MonoBehaviour
{
    [SerializeField] private UIController uiController = null;
    [SerializeField] private Transform mapRect = null;
    [SerializeField] private Transform QuestViewInfo = null;
    [SerializeField] private Transform DetailViewInfo = null;

    [SerializeField] private RectTransform QuestUIPrefab = null;
    [SerializeField] private RectTransform QuestTaskPrefab = null;
    [SerializeField] private RectTransform SpacerUIPrefab = null;
    [SerializeField] private GameObject QuestCategoryHeader = null;
    [SerializeField] private Transform questDetailPrefab = null;

    private bool First_run = true;
    private List<GameObject> tempList;
    private QuestsHolder QuestsHolder;
    private Dictionary<QuestCategory, Transform> questCategories = new Dictionary<QuestCategory, Transform>();
    private Color32 grayColor = new Color32(200, 200, 200, 255);


    public void GetDetails(int iter_in)
    {
        foreach (Transform iter in DetailViewInfo)
        {
            Destroy(iter.gameObject);
        }

        QuestTemplate tempActiveQuest = tempList[iter_in].GetComponent<QuestTemplate>();

        SetupComplex(tempActiveQuest);

        if (tempActiveQuest.questCategory != QuestCategory.Completed)
        {
            SetupTaskDetail(tempActiveQuest.returnActiveObjective());
        }

        Instantiate(SpacerUIPrefab, DetailViewInfo);

        List<QuestObjective> tempObjList = tempActiveQuest.returnCompletedObjectives();

        for(int i = tempObjList.Count - 1; i >= 0; i--)
        {
            SetupTaskDetail(tempObjList[i]);   
        }
    }

    public void QuestSetFocus(int iter_in)
    {
        QuestTemplate tempActiveQuest = tempList[iter_in].GetComponent<QuestTemplate>();
        if (tempActiveQuest.questCategory == QuestCategory.Completed)
        {
            return;
        }

        QuestsHolder.QuestSetFocus(tempList[iter_in]);
        UpdateFocusUI(QuestsHolder.ReturnFocus());
    }

    private void SetupComplex(QuestTemplate tempQuest)
    {
        Transform tempQuestDetail = Instantiate(questDetailPrefab, DetailViewInfo);
        tempQuestDetail.Find("Content").Find("QuestName").GetComponent<TextMeshProUGUI>().text = tempQuest.QuestName;
        tempQuestDetail.Find("Content").Find("Level").Find("Text").GetComponent<TextMeshProUGUI>().text = "LEVEL " + tempQuest.suggestedLevel.ToString();
        tempQuestDetail.Find("Content").Find("XP").Find("Text").GetComponent<TextMeshProUGUI>().text = "XP REWARD: " + tempQuest.xp_reward.ToString();
        tempQuestDetail.Find("Content").Find("Description").GetComponent<TextMeshProUGUI>().text = tempQuest.questDescription;
        DetailViewInfo.GetComponent<Animator>().Play("In");
    }

    public void LeaveDetailFocus()
    {
        DetailViewInfo.GetComponent<Animator>().Play("Out");
    }

    private void SetupTaskDetail(QuestObjective tempObj)
    {
        List<(bool, string)> taskList = tempObj.ReturnTasks();
        foreach ((bool, string) task in taskList)
        {
            Transform textPrefab = Instantiate(QuestTaskPrefab, DetailViewInfo);

            if (task.Item1)
            {
                textPrefab.Find("Text").GetComponent<TextMeshProUGUI>().color = grayColor;
                textPrefab.Find("Bullet").GetComponent<Image>().color = grayColor;
            }
            else
            {
                textPrefab.Find("Text").GetComponent<TextMeshProUGUI>().color = Color.white;
                textPrefab.Find("Bullet").GetComponent<Image>().color = Color.white;
            }

            textPrefab.Find("Text").GetComponent<TextMeshProUGUI>().text = task.Item2;
        }
    }

    private void OnEnable()
    {

        if (First_run)
        {
            first_run_func();
        }
        InstantiateQuestCategories();
        SetupQuests();
        mapRect.localPosition = new Vector3(1437.5f, 582.5f);
    }

    private void InstantiateQuestCategories()
    {
        foreach(QuestCategory questCategory in Enum.GetValues(typeof(QuestCategory)))
        {
            GameObject temp = Instantiate(QuestCategoryHeader, QuestViewInfo);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = questCategory.ToString() + " Quests";
            questCategories.Add(questCategory, temp.transform);
        }
    }

    private void SetupQuests()
    {
        tempList = QuestsHolder.ReturnActiveQuests();
        List<GameObject> completedList = QuestsHolder.ReturnCompletedQuests();

        int iter = 0;
        foreach (GameObject quest in tempList)
        {
            Transform tempPrefab = Instantiate(QuestUIPrefab, questCategories[quest.GetComponent<QuestTemplate>().questCategory]);
            tempPrefab.GetComponent<QuestNameUIPrefab>().Setup(quest, iter);
            iter += 1;
        }

        foreach (GameObject quest in completedList)
        {
            tempList.Add(quest);
            Transform tempPrefab = Instantiate(QuestUIPrefab, questCategories[QuestCategory.Completed]);
            tempPrefab.GetComponent<QuestNameUIPrefab>().Setup(quest, iter);
            iter += 1;
        }

        foreach (Transform transform in questCategories.Values)
        {
            if (transform.GetComponentsInChildren<QuestNameUIPrefab>().Length == 0)
            {
                transform.gameObject.SetActive(false);
            }
        }

        UpdateFocusUI(QuestsHolder.ReturnFocus());
    }

    private void UpdateFocusUI(GameObject temp)
    {
        foreach(Transform child in QuestViewInfo)
        {
            foreach(QuestNameUIPrefab quest in GetComponentsInChildren<QuestNameUIPrefab>())
            {
                quest.CheckFocus(temp);
            }
        }
    }

    private void first_run_func()
    {
        QuestsHolder = GameObject.Find("QuestsHolder").GetComponent<QuestsHolder>();
        First_run = false;
    }

    private void OnDisable()
    {
        uiController.ReturnMapLocation(mapRect);
        questCategories.Clear();

        foreach (Transform iter in QuestViewInfo)
        {
            Destroy(iter.gameObject);
        }

        foreach (Transform iter in DetailViewInfo)
        {
            Destroy(iter.gameObject);
        }
    }
}
