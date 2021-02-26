using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoFlame : AmmoMaster
{
    [SerializeField] private GameObject Exp;

    private void OnDisable()
    {
        GameObject temp = Instantiate(Exp, transform.parent);
        temp.transform.position = transform.position;
        Destroy(temp, 5f);
    }
}
