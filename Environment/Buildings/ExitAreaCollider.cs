using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitAreaCollider : MonoBehaviour
{
    [SerializeField] private BuildingController buildingController = null;
    [SerializeField] private BuildingController mainBuildingController = null;

    private bool insideBuilding = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other is CapsuleCollider && !insideBuilding)
        {
            buildingController.ExitCollider();
        }
    }

    private void Update()
    {
        insideBuilding = mainBuildingController.insideBuilding;
    }
}
