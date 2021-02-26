using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : Ability
{
    [SerializeField] GameObject grenade;
    [SerializeField] float DistMax = 10f;
    [SerializeField] float ForcePerDistance = 2f;

    private Transform projectileParent;
    private Transform LaunchPoint;

    private bool LateStart = true;


    void LateStartFunc()
    {
        if (LateStart)
        {
            LaunchPoint = GameObject.Find("Player").transform.FindDeepChild("RightHand");
            projectileParent = GameObject.Find("PlayerProjectiles").transform;

            if (LaunchPoint)
            {
                LateStart = false;
            }
        }
    }

    protected override void Attack()
    {
        LateStartFunc();

        GameObject temp = Instantiate(grenade, projectileParent);
        temp.transform.position = LaunchPoint.position;

        Vector3 Dir = cursorLogic.ReturnPlayer2Cursor();
        float dir_mag = Dir.magnitude;

        Dir = new Vector3(Dir.x, angle_finder(dir_mag), Dir.z);

        if(Dir.magnitude > DistMax)
        {
            Dir = Dir * DistMax / Dir.magnitude;
        }

        Dir *= ForcePerDistance;


        temp.GetComponent<Rigidbody>().velocity = Dir;
        temp.GetComponent<Grenade>().Setup();
    }

    private float angle_finder(float dir_mag)
    {
        //0 to DistMax units => 0 to 22.5 degrees

        if (dir_mag < DistMax)
        {
            return dir_mag * dir_mag / (2f * DistMax);
        }
        else
        {
            return dir_mag / 2f;
        }



    }
}
