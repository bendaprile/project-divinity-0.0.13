using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform enemy = null;
    [SerializeField] protected FactionsEnum Faction = FactionsEnum.Rogue;
    [SerializeField] protected int Level = -1; //If set will override max_enemy
    [SerializeField] protected int max_enemy = 1;
    [SerializeField] bool roam = false;
    [SerializeField] float X_range = 10;
    [SerializeField] float Z_range = 10;
    [SerializeField] float movement_cd = 4;
    [SerializeField] float cd_randomness = 2;
    [SerializeField] float roam_speed = 4;
    [SerializeField] float disable_range = 150;

    private GameObject player;
    protected bool spawner_enabled;

    private QuestsHolder QH;
    private int Current_EnemyCount = 0;

    private GameObject QuestRef;
    private int QuestInt;

    public void ChildDied() //This is so sad - Zach 2/2/2021
    {
        Current_EnemyCount -= 1;
        //Debug.Log(Current_EnemyCount);
        if(Current_EnemyCount == 0)
        {
            QH.CheckTaskCompletion(QuestRef, QuestInt);
        }
    }

    public void AttachToQuest(bool en, GameObject QuestRef_in, int QuestInt_in)
    {
        QuestRef = QuestRef_in;
        QuestInt = QuestInt_in;

        if (en)
        {
            GetComponent<Collider>().enabled = true;
        }
        else if (Current_EnemyCount == 0) //DO not check if just enabling
        {
            QH.CheckTaskCompletion(QuestRef, QuestInt);
        }
    }

    public int Return_EnemyCount()
    {
        if (gameObject.activeSelf)
        {
            return Current_EnemyCount;
        }
        else
        {
            return -1; //Unknown
        }
    }

    protected virtual void Awake()
    {
        spawner_enabled = false;
        QH = FindObjectOfType<QuestsHolder>();
        player = GameObject.Find("Player");
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !spawner_enabled)
        {
            Replenish_enemies();
            Current_EnemyCount = max_enemy;
            spawner_enabled = true;
        }
    }

    protected void FixedUpdate()
    {
        if (spawner_enabled)
        {
            float distance = (player.transform.position - transform.position).magnitude;
            if(distance >= disable_range)
            {
                spawner_enabled = false;
            }
        }
    }


    protected virtual void AdditionalSetup(int i, Transform clone)
    {
       
    }


    protected void Replenish_enemies()
    {
        int childSqrtRoot = (int)Mathf.Sqrt(max_enemy);

        for (int i = Current_EnemyCount; i < max_enemy; i++)
        {
            int x = (2 * (i % childSqrtRoot)) - childSqrtRoot;
            int z = (2 * (i / childSqrtRoot)) - childSqrtRoot;

            Vector3 modTrans = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            Transform clone = Instantiate(enemy, modTrans, transform.rotation);
            clone.GetComponent<EnemyTemplateMaster>().SpawnEnemy(Faction, roam, transform, X_range, Z_range, movement_cd, cd_randomness, roam_speed);
            clone.gameObject.SetActive(true);
            AdditionalSetup(i, clone); //After so Awake is called first
        } 
    }
}
