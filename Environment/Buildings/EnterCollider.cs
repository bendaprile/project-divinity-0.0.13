using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCollider : MonoBehaviour
{

    [SerializeField] private BuildingController buildingController = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other is CapsuleCollider)
        {
            buildingController.EnterCollider();
        }
    }
}
