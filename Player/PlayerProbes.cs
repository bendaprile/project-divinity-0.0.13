using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerProbes : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] Cameras = new CinemachineVirtualCamera[4];
    [SerializeField] private float MaxDist = 18f;
    [SerializeField] private float DistChangeMult = 1f;

    private bool AdvEnabled;
    private Transform Player;

    private float[] Angles = new float[4] { 50f, 60f, 75f, 89f };
    private int layerMask;

    float final_dist = 0;
    int iter = 0;

    public void enableDisable(bool val)
    {
        AdvEnabled = val;
        if (!AdvEnabled)
        {
            ResetAdvCams();
        }
    }

    private void ResetAdvCams()
    {
        foreach (CinemachineVirtualCamera cam in Cameras)
        {
            cam.Priority = 1;
        }
    }


    void Start()
    {
        layerMask = (1 << LayerMask.NameToLayer("Obstacles")) | (1 << LayerMask.NameToLayer("Terrain"));
        Player = GameObject.Find("Player").transform;
        final_dist = MaxDist;
    }

    private void Update()
    {
        if (!AdvEnabled)
        {
            return;
        }

        float cam_dist = Cameras[iter].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
        if (cam_dist < final_dist - .1f)
        {
            Cameras[iter].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance += DistChangeMult * (final_dist - cam_dist + 1) * Time.deltaTime;
        }
        else if (cam_dist > final_dist + .1f)
        {
            Cameras[iter].GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= DistChangeMult * (cam_dist - final_dist + 1) * Time.deltaTime;
        }

        ResetAdvCams();
        Cameras[iter].Priority = 10;
    }

    void FixedUpdate()
    {
        if (!AdvEnabled)
        {
            return;
        }

        final_dist = 0;
        for (int i = 0; i < 4; ++i)
        {
            float y_length = Mathf.Sqrt(2) * Mathf.Tan(Angles[i] * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(-1f, y_length, -1f);

            float double_dist = 0; //Average of the two distances
            for(int j = 0; j < 2; ++j)
            {
                Vector3 RayCasPos = Player.position + Vector3.up * (1 - (j * 2));
                RaycastHit hit;

                if (Physics.Raycast(RayCasPos, dir, out hit, MaxDist, layerMask))
                {
                    //Debug.Log((hit.collider.name, LayerMask.LayerToName(hit.collider.gameObject.layer)));
                    double_dist += (hit.point - RayCasPos).magnitude;
 
                }
                else
                {
                    double_dist += MaxDist;
                }
            }

            if (double_dist > final_dist)
            {
                iter = i;
                final_dist = double_dist;
            }
        }

        final_dist /= 2;
    }
}
