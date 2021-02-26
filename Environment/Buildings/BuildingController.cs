using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Cinemachine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] public GameObject roof = null;
    [SerializeField] public GameObject lights = null;
    [SerializeField] private GameObject[] interiorObjectsToDisableWhenOutside = null;
    [SerializeField] private GameObject[] interiorObjectsToDisableWhenInside = null;
    [SerializeField] private List<AudioClip> AmbientAudio = null;

    [SerializeField] private bool lightsAsChildren = false;
    [SerializeField] protected bool roofShadowsOnly = false;
    public bool insideBuilding = false;
    [SerializeField] private float camDistance = 0f;
    
    private NonDiegeticController AudioControl;
    private CameraEnvironmentController controller;

    protected bool lateStart = true;

    private void Start()
    {
        AudioControl = GameObject.Find("Non Diegetic Audio").GetComponent<NonDiegeticController>();
        controller = FindObjectOfType<CameraEnvironmentController>();

        if (roof && roof.activeSelf == false)
        {
            roof.SetActive(true);
        }
    }

    private void LateUpdate()
    {
        if (lateStart) {
            lateStart = false;
            if (insideBuilding)
            {
                insideBuilding = false;
                EnterCollider();
            }
            else
            {
                if (lights) { ControlLights(false); }
                if (roof) { ControlRoof(true); }
                if (interiorObjectsToDisableWhenOutside.Length > 0) { EnableDisableInterior(false); }
                if (interiorObjectsToDisableWhenInside.Length > 0) { EnableDisableInterior(true); }
            }
        }
    }

    public virtual void EnterCollider()
    {
        if (insideBuilding == false)
        {
            if (roof) { ControlRoof(false); }
            if (lights) { ControlLights(true); }
            if (interiorObjectsToDisableWhenOutside.Length > 0) { EnableDisableInterior(true); }
            StartCoroutine(controller.EnterBuilding(!roofShadowsOnly, camDistance));
            insideBuilding = true;
            if (AmbientAudio != null)
            {
                AudioControl.ChangeAudioSpecific(AmbientAudio);
            }
        }
    }

    public virtual void ExitCollider()
    {
        if (insideBuilding == true)
        {
            if (roof) { ControlRoof(true); }
            if (lights) { ControlLights(false); }
            if (interiorObjectsToDisableWhenOutside.Length > 0) { EnableDisableInterior(false); }
            StartCoroutine(controller.ExitBuilding(!roofShadowsOnly));
            insideBuilding = false;
            AudioControl.ChangeAudioGeneral();
        }
    }

    private void ControlLights(bool turnOnLights)
    {
        if (!lightsAsChildren)
        {
            lights.SetActive(turnOnLights);
        }
        else
        {
            foreach (Transform child in lights.transform)
            {
                if (child.GetComponentInChildren<Light>())
                {
                    child.GetComponentInChildren<Light>().enabled = turnOnLights;
                }
            }
        }
    }

    private void ControlRoof(bool turnOnRoof)
    {
        if (!roofShadowsOnly)
        {
            roof.SetActive(turnOnRoof);
        }
        else
        {
            foreach (Transform child in roof.transform)
            {
                if (child.GetComponent<MeshRenderer>())
                {
                    child.GetComponent<MeshRenderer>().shadowCastingMode = turnOnRoof ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
        }
    }

    private void EnableDisableInterior(bool enable)
    {
        foreach (GameObject interior in interiorObjectsToDisableWhenOutside)
        {
            interior.SetActive(enable);
        }
    }
}
