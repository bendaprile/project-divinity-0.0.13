using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveThing : MonoBehaviour
{
    [SerializeField] protected bool LOCK_UNTIL_QUEST = false;
    [SerializeField] protected Transform QuestIndicator = null;

    protected GameObject player;
    protected bool isCursorOverhead;
    protected PlayerInRadius PIR;
    protected GameObject Text;
    protected UIController UIControl;
    protected TextMeshPro overHead_t_mesh;
    protected bool fixedName = false;

    protected bool MiscDisplayUsed;


    protected GameObject QuestRef;
    protected int QuestInt;



    protected virtual void Awake()
    {
        if (!MiscDisplayUsed)
        {
            Text = transform.Find("Text").gameObject;
            overHead_t_mesh = GetComponentInChildren<TextMeshPro>();
        }

        PIR = GetComponentInChildren<PlayerInRadius>();
        player = GameObject.Find("Player");
        UIControl = GameObject.Find("UI").GetComponent<UIController>();
    }

    public void SetQuest(GameObject QuestRef_in, int i)
    {
        LOCK_UNTIL_QUEST = false;
        QuestInt = i;
        QuestRef = QuestRef_in;
    }


    public virtual void CursorOverObject()
    {
        isCursorOverhead = true;
    }

    public virtual void CursorLeftObject()
    {
        isCursorOverhead = false;
    }

    protected virtual void ActivateLogic()
    {

    }

    protected void QuestActiveLogic()
    {
        if (QuestRef)
        {
            GameObject.Find("QuestsHolder").GetComponent<QuestsHolder>().CheckTaskCompletion(QuestRef, QuestInt);
            QuestRef = null;
        }
    }

    protected virtual void Update()
    {
        if (LOCK_UNTIL_QUEST)
        {
            return;
        }

        if ((isCursorOverhead && PIR.isTrue) || fixedName)
        {
            if (Text)
            {
                Text.SetActive(true);
                rotate(Text.transform);
            };
            ActivateLogic();
        }
        else
        {
            if (Text) { Text.SetActive(false); };
        }

        if (QuestIndicator) //There might be dia without an indicator like using a computer
        {
            if (HasTask() && !fixedName)
            {
                QuestIndicator.gameObject.SetActive(true);
                QuestIndicator.GetComponent<SpriteRenderer>().color = Color.white;
                rotate(QuestIndicator);
            }
            else if (StartsQuest() && !fixedName)
            {
                QuestIndicator.gameObject.SetActive(true);
                QuestIndicator.GetComponent<SpriteRenderer>().color = STARTUP_DECLARATIONS.goldColor;
                rotate(QuestIndicator);
            }
            else
            {
                QuestIndicator.gameObject.SetActive(false);
            }
        }
    }

    protected virtual bool HasTask()
    {
        return QuestRef != null;
    }

    protected virtual bool StartsQuest()
    {
        return false;
    }

    protected virtual void rotate(Transform RotateObj)
    {
        RotateObj.transform.eulerAngles = new Vector3(RotateObj.transform.eulerAngles.x, 45f, RotateObj.transform.eulerAngles.z);
    }
}
