using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShadowController : MonoBehaviour
{
    public float frontTriggerDistance = 75f;
    public float behindTriggerDistance = 40f;
    public bool onlyEnableAtNight = true;
    public GameObject[] lights = null;
    public float checkDistanceFrequency = 0.5f;
    private float distance;
    private GameObject Player;
    private DayNightController dayNight;
    private float counter = 0f;

    void Start()
    {
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
            EnableDisableLights(false);
            return;
        }

        bool Lightinfront = (Player.transform.position.x - transform.position.x) + (Player.transform.position.z - transform.position.z) < 0;
        distance = Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.z), new Vector2(transform.position.x, transform.position.z));

        // If light is in front of the player use forward trigger distance
        if (Lightinfront)
        {
            if (distance < frontTriggerDistance)
            {
                EnableDisableLights(true);
            }
            if (distance > frontTriggerDistance)
            {
                EnableDisableLights(false);
            }
        }
        // If light is behind the player use backward trigger distance
        else
        {
            if (distance < behindTriggerDistance)
            {
                EnableDisableLights(true);
            }
            if (distance > behindTriggerDistance)
            {
                EnableDisableLights(false);
            }
        }
    }

    private void EnableDisableLights(bool enable)
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(enable);
        }
    }
}
