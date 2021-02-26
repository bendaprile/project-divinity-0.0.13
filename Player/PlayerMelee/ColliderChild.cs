using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderChild : MonoBehaviour
{
    public List<Collider> TriggerList = new List<Collider>();

    void OnTriggerEnter(Collider other)
    {
        if (!TriggerList.Contains(other))
        {
            if (other.gameObject.tag == "BasicEnemy")
            {
                TriggerList.Add(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (TriggerList.Contains(other))
        {
            if (other.gameObject.tag == "BasicEnemy")
            {
                TriggerList.Remove(other);
            }
        }
    }
}
