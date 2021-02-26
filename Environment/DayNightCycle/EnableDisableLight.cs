using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableLight : MonoBehaviour
{
    public float frontTriggerDistance = 75f;
    public float behindTriggerDistance = 35f;
    public bool onlyEnableAtNight = true;
    private float Distance;
    public float checkDistanceFrequency = 0.5f;
    private Light lightComp;
    private GameObject Player;
    private DayNightController dayNight;
    private float counter = 0f;

    void Start()
    {
        lightComp = gameObject.GetComponent<Light>();
        Player = GameObject.FindGameObjectWithTag("Player");
        dayNight = FindObjectOfType<DayNightController>();
    }

    void Update()
    {
        if (counter < checkDistanceFrequency)
        {
            counter += Time.deltaTime;
            return;
        }
        counter = 0f;

        if (onlyEnableAtNight && dayNight.isLightOutside)
        {
            lightComp.enabled = false;
            return;
        }

        bool Lightinfront = (Player.transform.position.x - transform.position.x) + (Player.transform.position.z - transform.position.z) < 0;
        Distance = Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.z), new Vector2(transform.position.x, transform.position.z));

        // If light is in front of the player use forward trigger distance
        if (Lightinfront)
        {
            if (Distance < frontTriggerDistance)
            {
                lightComp.enabled = true;
            }
            if (Distance > frontTriggerDistance)
            {
                lightComp.enabled = false;
            }
        }
        // If light is behind the player use backward trigger distance
        else
        {
            if (Distance < behindTriggerDistance)
            {
                lightComp.enabled = true;
            }
            if (Distance > behindTriggerDistance)
            {
                lightComp.enabled = false;
            }
        }
    }
}

