using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class QuestObjective : MonoBehaviour
{
    [SerializeField] protected GameObject Enable_Gameobject_Start = null; //Use this to help setup events

    [SerializeField] protected int NumberOfTasks = 1;

    [SerializeField] private bool[] FakeTask = new bool[1] { false };
    [SerializeField] protected string[] TaskDescription = new string[1] { "" };

    [SerializeField] protected GameObject Fail_Flag = null;

    ///LOCATION
    [SerializeField] protected bool LocationHint = false;
    [SerializeField] protected List<Transform> ExactLocation = new List<Transform>(); //EXCLUSIVE
    [SerializeField] protected List<Vector2> CenterPoint = new List<Vector2>(); //EXCLUSIVE
    [SerializeField] protected List<float> Radius = new List<float>();
    ///LOCATION


    private int NumberOfTasks_Real;
    private int TaskCompleteCount = 0;
    protected bool[] TaskCompleted;
    protected QuestTemplate questTemplate;

    public virtual void initialize()
    {
        TaskCompleted = new bool[NumberOfTasks];
        Assert.IsTrue((ExactLocation.Count == 0) || (CenterPoint.Count == 0));
        Assert.IsTrue(NumberOfTasks == TaskCompleted.Length);
        Assert.IsTrue(NumberOfTasks == FakeTask.Length);
        Assert.IsTrue(NumberOfTasks == TaskDescription.Length);
        if (LocationHint)
        {
            Assert.IsTrue((ExactLocation.Count == NumberOfTasks) || (CenterPoint.Count == NumberOfTasks));
        }

        for (int i = 0; i < NumberOfTasks; ++i)
        {
            NumberOfTasks_Real += FakeTask[i] ? 0 : 1;
        }

        if (Enable_Gameobject_Start)
        {
            Assert.IsFalse(Enable_Gameobject_Start.activeSelf);
            Enable_Gameobject_Start.SetActive(true);
        }

        questTemplate = GetComponentInParent<QuestTemplate>();
        FlagCheckObj();
    }

    public virtual List<(bool, string)> ReturnTasks()
    {
        List<(bool, string)> taskList = new List<(bool, string)>();
        for (int i = 0; i < NumberOfTasks; ++i)
        {
            taskList.Add((TaskCompleted[i], TaskDescription[i]));
        }
        return taskList;
    }

    public virtual ObjectiveType ReturnType()
    {
        return ObjectiveType.NOTHING; 
    }

    public virtual (bool, List<(Vector2, float)>) ReturnLocs()
    {
        if (LocationHint)
        {
            return (false, ReturnLocsHelper());
        }
        else
        {
            return (false, new List<(Vector2, float)>());
        }
    }

    protected virtual void AdditionalTaskCompletionLogic(int num)
    {

    }

    protected virtual void AdditionalObjectiveCompletionLogic()
    {

    }

    //FIXED FUNCTIONS
    public void TaskCompletion(GameObject ObjectiveRef, int num)
    {
        if (ObjectiveRef == gameObject)
        {
            TaskCompletion_Self(num);
        }
    }

    public void TaskCompletion_Self(int num)
    {
        if (!TaskCompleted[num])
        {
            TaskCompleted[num] = true;
            if (!FakeTask[num])
            {
                AdditionalTaskCompletionLogic(num);
                TaskCompleteCount += 1;
            }
        }

        if (TaskCompleteCount == NumberOfTasks_Real)
        {
            AdditionalObjectiveCompletionLogic();
            questTemplate.ObjectiveFinished();
        }
    }

    public void FlagCheckObj()
    {
        if (Fail_Flag && questTemplate.ZF.CheckFlag(Fail_Flag.name))
        {
            questTemplate.ObjectiveFinished();
        }
    }

    protected List<(Vector2, float)> ReturnLocsHelper()
    {
        List<(Vector2, float)> TempList = new List<(Vector2, float)>();
        for (int i = 0; i < CenterPoint.Count; i++)
        {
            float Rad = (Radius.Count > i) ? Radius[i] : 0;
            if (!TaskCompleted[i])
            {
                TempList.Add((CenterPoint[i], Rad));
            }
        }

        for (int i = 0; i < ExactLocation.Count; i++)
        {
            float Rad = (Radius.Count > i) ? Radius[i] : 0;
            if (!TaskCompleted[i])
            {
                Vector2 tempExact = new Vector2(ExactLocation[i].position.x, ExactLocation[i].position.z);
                TempList.Add((tempExact, Rad));
            }
        }
        return TempList;
    }
    ////////////////////
}
