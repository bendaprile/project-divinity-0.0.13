using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class InteractiveDia : InteractiveThing
{
    [SerializeField] private Transform DiaTrans = null;
    [SerializeField] private DiaRoot DR = null;

    private bool in_combat;
    private string Temp_text;
    private float Original_size;

    public virtual void SetText(string str)
    {
        overHead_t_mesh.text = str;
    }

    protected override void Awake()
    {
        Assert.IsFalse(LOCK_UNTIL_QUEST); //DO NOT USE FOR THIS (No reason not to, but not needed... yet..)
        base.Awake();
        in_combat = false;
        Original_size = overHead_t_mesh.fontSize;
    }

    public void CombatText(float sizeMult)
    {
        fixedName = true;
        in_combat = true; 
        overHead_t_mesh.fontSize = Original_size * sizeMult;
    }

    public void NormalText()
    {
        fixedName = false;
        in_combat = false;
        overHead_t_mesh.fontSize = Original_size;
    }

    private IEnumerator DisplayDia(string str)
    {
        fixedName = true;
        Temp_text = overHead_t_mesh.text;
        SetText(str);

        float timer = 5;
        while(timer > 0)
        {
            if (in_combat)
            {
                SetText(Temp_text);
            }
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        fixedName = false;
        SetText(Temp_text);
    }

    protected override void ActivateLogic()
    {
        if (Input.GetKeyDown(KeyCode.E) && !fixedName)
        {
            ForcedActivate();
        }
    }

    public void ForcedActivate()
    {
        QuestActiveLogic();
        Transform StartingTrans = DiaTrans.GetComponent<DiaRoot>().ReturnStarting();
        DiaOverhead diaOver = StartingTrans.GetComponent<DiaOverhead>();
        if (diaOver)
        {
            StartCoroutine(DisplayDia(diaOver.return_line()));
        }
        else
        {
            UIControl.DialogueMenuBool(DiaTrans);
        }
    }

    protected override bool HasTask()
    {
        return DR.dia_quest_objective_count > 0;
    }

    protected override bool StartsQuest()
    {
        return DR.has_quest;
    }
}
