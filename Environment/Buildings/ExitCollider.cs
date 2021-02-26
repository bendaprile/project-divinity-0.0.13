using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCollider : MonoBehaviour
{

    [SerializeField] private BuildingController buildingController = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other is CapsuleCollider)
        {
            buildingController.ExitCollider();
        }
    }
}
