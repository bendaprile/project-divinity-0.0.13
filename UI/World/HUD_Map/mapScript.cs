using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapScript : MonoBehaviour
{
    [SerializeField] private float angleoffset = -45f; // x or y size
    [SerializeField] private float mapsize = 0; // x or y size
    [SerializeField] private float Worldmapsize = 0; //One edge
    [SerializeField] private Vector2 TopPoint = new Vector2(); //X then Z
    [SerializeField] private Transform ObjParent = null;
    [SerializeField] private GameObject Circle = null;
    [SerializeField] private GameObject Point = null;

    private float length;
    private Vector2 MapCenter;
    private Transform playerLoc;
    private RectTransform playerIconUI;
    private QuestsHolder quests;

    private bool first_start = true;


    void First_Start()
    {
        playerLoc = GameObject.Find("Player").transform;
        playerIconUI = transform.Find("FullMapHolder").Find("PlayerLocation").GetComponent<RectTransform>();
        quests = GameObject.Find("QuestsHolder").GetComponent<QuestsHolder>();

        length = Worldmapsize / 2;

        MapCenter = new Vector2(TopPoint.x - length, TopPoint.y - length);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentloc = new Vector2(playerLoc.position.x - MapCenter.x, playerLoc.position.z - MapCenter.y);
        currentloc *= (mapsize / (Worldmapsize));

        Vector2 rotatedloc;
        rotatedloc.x = (currentloc.x - currentloc.y) / 2;
        rotatedloc.y = (currentloc.x + currentloc.y) / 2;

        playerIconUI.localPosition = new Vector3(rotatedloc.x, rotatedloc.y, 0);

        playerIconUI.localEulerAngles = new Vector3(0f, 0f, -(playerLoc.localEulerAngles.y + angleoffset));
    }

    void OnEnable()
    {
        if (first_start)
        {
            First_Start();
            first_start = false;
        }
        UpdateObjLoc();
    }

    public void UpdateObjLoc()
    {
        foreach(Transform temp in ObjParent)
        {
            Destroy(temp.gameObject);
        }

        GameObject tempObj = quests.ReturnFocus();
        if (tempObj != null)
        {
            QuestTemplate tempScipt = tempObj.GetComponent<QuestTemplate>();
            Color objColor;
            if (tempScipt.returnActiveLocs().Item1) //Forced
            {
                objColor = new Color(1, 0, 0, .5f);
            }
            else //Hint
            {
                objColor = new Color(0, 0, 1, .5f);
            }


            foreach ((Vector2, float) loc in tempScipt.returnActiveLocs().Item2)
            {
                Vector2 startingLoc = new Vector2(loc.Item1.x - MapCenter.x, loc.Item1.y - MapCenter.y);
                startingLoc *= (mapsize / (Worldmapsize));

                Vector2 rotatedObjloc;
                rotatedObjloc.x = (startingLoc.x - startingLoc.y) / 2;
                rotatedObjloc.y = (startingLoc.x + startingLoc.y) / 2;

                GameObject temp;
                if (loc.Item2 >= 100)
                {
                    temp = Instantiate(Circle, ObjParent);
                    float size = loc.Item2 * Mathf.Sqrt(2) * (mapsize / (Worldmapsize));
                    temp.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                }
                else
                {
                    objColor.a = 1f;
                    temp = Instantiate(Point, ObjParent);
                }

                temp.GetComponent<Image>().color = objColor;
                temp.GetComponent<RectTransform>().localPosition = new Vector3(rotatedObjloc.x, rotatedObjloc.y, 0f);


            }
        }
    }
}
