using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLightController : BuildingController
{

    private void Start()
    {
        if (insideBuilding)
        {
            if (lights) { lights.SetActive(true); }
            if (roof) { ControlRoof(false); }
        } 
        else
        {
            if (lights) { lights.SetActive(false); }
            if (roof) { ControlRoof(true); }
        }
        lateStart = false;
    }

    public override void EnterCollider()
    {
        if (insideBuilding == false)
        {
            if (roof) { ControlRoof(false); }
            if (lights) { lights.SetActive(true); }
            insideBuilding = true;
        }
    }

    public override void ExitCollider()
    {
        if (insideBuilding == true)
        {
            if (roof) { ControlRoof(true); }
            if (lights) { lights.SetActive(false); }
            insideBuilding = false;
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
}
