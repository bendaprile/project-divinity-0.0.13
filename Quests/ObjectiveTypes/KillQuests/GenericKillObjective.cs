using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GenericKillObjective : QuestObjective
{
    [SerializeField] private GameObject[] enemyNeeded = new GameObject[1];
    [SerializeField] private int[] enemyNumbers = new int[1];

    [SerializeField] private bool LocationRequired = false;

    private List<EnemyTemplateMaster> enemyNeededSpecs = new List<EnemyTemplateMaster>();
    private List<int> currentKills = new List<int>();

    public override void initialize()
    {
        Assert.AreEqual(NumberOfTasks, enemyNeeded.Length);
        Assert.AreEqual(NumberOfTasks, enemyNumbers.Length);

        base.initialize();
        for (int i = 0; i < NumberOfTasks; i++)
        {
            currentKills.Add(0);
            enemyNeededSpecs.Add(enemyNeeded[i].GetComponent<EnemyTemplateMaster>());
        }
    }

    public void UpdateKillCounts(EnemyTemplateMaster etm, Vector2 loc)
    {
        for (int i = 0; i < enemyNeeded.Length; i++)
        {
            if (enemyNeededSpecs[i].Return_EnemyTypeName() == etm.Return_EnemyTypeName())
            {
                bool conditionsMet = false;
                if (LocationRequired)
                {
                    Vector2 LocTemp = (CenterPoint.Count == NumberOfTasks) ? CenterPoint[i] : new Vector2(ExactLocation[i].position.x, ExactLocation[i].position.z);
                    float dist = Mathf.Sqrt(Mathf.Pow(LocTemp.x - loc.x, 2f) + Mathf.Pow(LocTemp.y - loc.y, 2f));
                    if(dist < Radius[i])
                    {
                        conditionsMet = true;
                    }
                }
                else
                {
                    conditionsMet = true;
                }

                if(conditionsMet && currentKills[i] < enemyNumbers[i])
                {
                    currentKills[i] += 1;

                    if(currentKills[i] == enemyNumbers[i])
                    {
                        TaskCompletion_Self(i);
                    }
                }
            }
        }
    }

    public override List<(bool, string)> ReturnTasks()
    {
        List<(bool, string)> taskList = new List<(bool, string)>();

        for (int i = 0; i < enemyNeeded.Length; i++)
        {
            string amount_completed = " (" + currentKills[i] + "/" + enemyNumbers[i] + ")";
            taskList.Add((TaskCompleted[i], (TaskDescription[i] + amount_completed)));
        }

        return taskList;
    }

    public override ObjectiveType ReturnType()
    {
        return ObjectiveType.GenericKill;
    }

    public override (bool, List<(Vector2, float)>) ReturnLocs()
    {
        if (LocationHint || LocationRequired)
        {
            return (LocationRequired, ReturnLocsHelper());
        }
        else
        {
            return (false, new List<(Vector2, float)>());
        }
    }
}
