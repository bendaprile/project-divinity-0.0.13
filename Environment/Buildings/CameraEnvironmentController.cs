using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraEnvironmentController : MonoBehaviour
{
    [SerializeField] Light sun = null;
    [SerializeField] Light moon = null;
    public float outdoorBrightness = 0;
    public float delay = 4f;

    private float decreaseSunIntensity;
    private float decreaseMoonIntensity;
    private CameraStateController camStateController;

    private void Start()
    {
        camStateController = FindObjectOfType<CameraStateController>();
        decreaseSunIntensity = sun.intensity - outdoorBrightness;
        decreaseMoonIntensity = moon.intensity - outdoorBrightness;
    }

    public IEnumerator EnterBuilding(bool decreaseOutdoorLight, float camDistance = 0)
    {

        if (camDistance > 0)
        {
            camStateController.CustomBuildingCamDistance(camDistance);
        }
        else
        {
            camStateController.SetCamState(CameraStateController.CameraState.BuildingCam);
        }

        if (decreaseOutdoorLight)
        {
            for (float i = 0; i < delay; i += Time.deltaTime)
            {
                if (decreaseOutdoorLight)
                {
                    sun.intensity -= decreaseSunIntensity * Time.deltaTime / delay;
                    moon.intensity -= decreaseMoonIntensity * Time.deltaTime / delay;
                }
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator ExitBuilding(bool decreaseOutdoorLight)
    {
        camStateController.SetCamState(CameraStateController.CameraState.AdventureCam);

        if (decreaseOutdoorLight)
        {
            for (float i = 0; i < delay; i += Time.deltaTime)
            {
                if (decreaseOutdoorLight)
                {
                    sun.intensity += decreaseSunIntensity * Time.deltaTime / delay;
                    moon.intensity += decreaseMoonIntensity * Time.deltaTime / delay;
                }
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForEndOfFrame();
    }
}
