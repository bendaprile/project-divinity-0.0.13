using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DayNightController : MonoBehaviour
{
    //Night 9pm to 5am

    [Range(0, 24)] [SerializeField] public float CurrentTime;
    public Light sun;
    public Light moon;

    [SerializeField] private bool PingPongShadows = true;
    [SerializeField] private float TimeMultiplier = 24;
    [SerializeField] private float SunStartingAngle = 1f;
    [SerializeField] private float MoonStartingAngle = 3f;
    [SerializeField] private float MoonLightAdder = 1f;
    [SerializeField] private float updateFrequency = 1f;

    public bool isNight;
    public bool isLightOutside;

    private float updateTimer;
    private float moonNominal;

    private void Start()
    {
        moonNominal = moon.intensity;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CurrentTime = (CurrentTime + (Time.fixedDeltaTime * TimeMultiplier) / 3600) % 24;
        updateTimer -= Time.fixedDeltaTime;

        if (updateTimer < 0)
        {
            UpdatePositions();
            updateTimer = updateFrequency;
        }
    }

    private void OnValidate()
    {
        UpdatePositions();
    }


    private void UpdatePositions()
    {
        CheckNightDayTransition();
        if (isNight)
        {
            float timeOfNight = (CurrentTime - 21) > 0 ? (CurrentTime - 21) : (CurrentTime + 3);
            float alpha = timeOfNight / 8f;

            float moonRotation;
            if (PingPongShadows)
            {
                moonRotation = Mathf.Lerp(180 - MoonStartingAngle, MoonStartingAngle, alpha);
            }
            else
            {
                moonRotation = Mathf.Lerp(MoonStartingAngle, 180 - MoonStartingAngle, alpha);
            }

            moon.transform.rotation = Quaternion.Euler(moonRotation, -45.0f, 0);
            moon.intensity = 1 + MoonLightAdder * Mathf.Pow(1 - Mathf.Sin(moonRotation * Mathf.Deg2Rad), 4); //Gross code tbh
            //Debug.Log((moonRotation * Mathf.Deg2Rad, moon.intensity));
        }
        else
        {
            float timeOfDay = CurrentTime - 5f;
            float alpha = timeOfDay / 16f;
            float sunRotation = Mathf.Lerp(SunStartingAngle, 180 - SunStartingAngle, alpha);
            sun.transform.rotation = Quaternion.Euler(sunRotation, -45.0f, 0);
        }
    }

    private void CheckNightDayTransition()
    {
        if(CurrentTime > 5 && CurrentTime < 21) //Is day
        {
            if (isNight)
            {
                StartDay();
            }
        }
        else
        {
            if (!isNight)
            {
                StartNight();
            }
        }

        if (CurrentTime > 6 && CurrentTime < 20)
        {
            isLightOutside = true;
        }
        else
        {
            isLightOutside = false;
        }
    }

    private void StartDay()
    {
        isNight = false;
        moon.enabled = false;
        sun.enabled = true;
    }

    private void StartNight()
    {
        isNight = true;
        moon.enabled = true;
        sun.enabled = false;
    }
}
