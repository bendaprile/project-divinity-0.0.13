using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRadius : MonoBehaviour
{
    public bool isTrue;
    void Start()
    {
        isTrue = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isTrue = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTrue = false;
        }
    }
}
