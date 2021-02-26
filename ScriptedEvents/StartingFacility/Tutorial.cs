using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    private enum LightingStage
    {
        Prometheus,
        PrometheusToPallas,
        Pallas,
        PallasToEpimetheus,
        Epimetheus,
        EpimetheusToPrometheus,
        End
    }

    private UIController UIControl;
    QuestsHolder QH;

    private Transform Player;
    private Inventory inventory;


    [SerializeField] private Transform Quest = null;
    [SerializeField] private List<GameObject> QuestObjectives = null;

    [SerializeField] private Transform Prom_Initial = null;
    [SerializeField] private InteractiveBox Box_w_items = null;
    [SerializeField] private Transform Pallas_post_box = null;
    [SerializeField] private Transform Pallas_fighting = null;
    [SerializeField] private Transform Pallas_final = null;

    [SerializeField] private GameObject ZomSpawner = null;

    [SerializeField] private Collider initalPallasTrigger = null;
    [SerializeField] private Collider initalEpimetheusTrigger = null;
    [SerializeField] private Collider leavePrometheusRoom = null;
    [SerializeField] private Collider leavePallasRoom = null;
    [SerializeField] private Collider leaveEpimetheusRoom = null;
    [SerializeField] private Collider prometheusRoom2 = null;

    [SerializeField] private SlidingDoorController ExteriorDoor = null;
    [SerializeField] private SlidingDoorController PallasNearDoor = null;
    [SerializeField] private SlidingDoorController PallasFarDoor = null;
    [SerializeField] private SlidingDoorController EpimetheusDoor = null;
    [SerializeField] private Transform PrometheusToPallas = null;
    [SerializeField] private Transform PallasToEpimetheus = null;
    [SerializeField] private Transform EpimetheusToPrometheus = null;
    [SerializeField] private Transform TheRest = null;
    [SerializeField] private Transform EntryLights = null;

    [SerializeField] private Material emissiveMaterial = null;
    [SerializeField] private Material nonEmissiveMaterial = null;

    int LateCounter = 0;
    private DroneController DC;
    private QuestTemplate questTemplate;
    private LightingStage lightingStage = LightingStage.Prometheus;

    void Start()
    {
        UIControl = GameObject.Find("UI").GetComponent<UIController>();
        Player = GameObject.Find("Player").transform;
        QH = GameObject.Find("QuestsHolder").GetComponent<QuestsHolder>();
        DC = GameObject.Find("Drone").GetComponent<DroneController>();
        inventory = Player.GetComponentInChildren<Inventory>();
        questTemplate = Quest.GetComponent<QuestTemplate>();

        TriggerLightsSegmented(GetFirstChildren(PrometheusToPallas), false);
        TriggerLightsSegmented(GetFirstChildren(PallasToEpimetheus), false);
        TriggerLightsSegmented(GetFirstChildren(EpimetheusToPrometheus), false);
        TriggerLights(TheRest.GetComponentsInChildren<MeshRenderer>(), false);
        TriggerLights(EntryLights.GetComponentsInChildren<MeshRenderer>(), false);
    }



    private bool QO2_first = true;
    private bool QO3_first = true;
    private bool QO4_first = true;

    void Update()
    {
        if(LateCounter == 2)
        {
            QH.AddQuest(Quest);
            QH.QuestSetFocus(Quest.gameObject);
            UIControl.DialogueMenuBool(Prom_Initial);
            LateCounter = -1;
        }

        if(LateCounter != -1)
        {
            LateCounter += 1;
        }

        if (!UIControl.GamePaused)
        {
            ControlHallLights();


            if (questTemplate.CompareObj(QuestObjectives[1]))
            {
                if (!initalPallasTrigger.enabled)
                {
                    PallasNearDoor.ModifyLarcenyReq(90); //lock in room
                }
            }


            if (questTemplate.CompareObj(QuestObjectives[2]))
            {
                if (QO2_first)
                {
                    UIControl.DialogueMenuBool(Pallas_post_box);
                    QO2_first = false;
                }

                GameObject w1 = inventory.ReturnWeapon(0);
                GameObject w2 = inventory.ReturnWeapon(1);

                if (w1 != null || w2 != null)
                {
                    QH.CheckTaskCompletion(QuestObjectives[2], 0);
                }
            }


            if (questTemplate.CompareObj(QuestObjectives[3]))
            {
                if (QO3_first)
                {
                    UIControl.DialogueMenuBool(Pallas_fighting);
                    ZomSpawner.SetActive(true);
                    QO3_first = false;
                }
            }


            if (questTemplate.CompareObj(QuestObjectives[4]))
            {
                if (QO4_first)
                {
                    UIControl.DialogueMenuBool(Pallas_final);
                    PallasFarDoor.ModifyLarcenyReq(0);
                    EpimetheusDoor.ModifyLarcenyReq(0);
                    QO4_first = false;
                }
            }

            if (!initalEpimetheusTrigger.enabled)
            {
                DC.ChangeDroneMode(true);
            }
        }
    }

    private void ControlHallLights()
    {
        if (lightingStage == LightingStage.End)
        {
            return;
        }
        
        if (!leavePrometheusRoom.enabled && lightingStage == LightingStage.Prometheus)
        {
            // Turn on hall lights from Prometheus to Pallas' Room
            StartCoroutine(TriggerLightsCoroutine(GetFirstChildren(PrometheusToPallas), true));
            lightingStage = LightingStage.PrometheusToPallas;
            ExteriorDoor.ModifyLarcenyReq(90);
            EpimetheusDoor.ModifyLarcenyReq(90);
        }

        if (!initalPallasTrigger.enabled && lightingStage == LightingStage.PrometheusToPallas)
        {
            // Turn off hall lights from PrometheusToPallas and set lighting stage to PrometheusToPallas
            TriggerLightsSegmented(GetFirstChildren(PrometheusToPallas), false);
            lightingStage = LightingStage.Pallas;
            PrometheusToPallas.Find("LightSegment").SetParent(EpimetheusToPrometheus);
        }

        if (!leavePallasRoom.enabled && lightingStage == LightingStage.Pallas)
        {
            // Turn on hall lights from Pallas to Epimetheus
            StartCoroutine(TriggerLightsCoroutine(GetFirstChildren(PallasToEpimetheus), true));
            lightingStage = LightingStage.PallasToEpimetheus;
        }

        if (!initalEpimetheusTrigger.enabled && lightingStage == LightingStage.PallasToEpimetheus)
        {
            // Turn off lights from PrometheusToPallas and set lighting stage to PallasToEpimetheus
            TriggerLightsSegmented(GetFirstChildren(PallasToEpimetheus), false);
            lightingStage = LightingStage.Epimetheus;
            leaveEpimetheusRoom.enabled = true;
            Transform tempLightSegement = PallasToEpimetheus.Find("LightSegment (8)");
            tempLightSegement.SetParent(EpimetheusToPrometheus);
            tempLightSegement.SetAsFirstSibling();
        }

        if (!leaveEpimetheusRoom.enabled && lightingStage == LightingStage.Epimetheus)
        {
            // Turn on lights from EpimetheusToPallas and set lighting stage to EpimetheusToPrometheus
            StartCoroutine(TriggerLightsCoroutine(GetFirstChildren(EpimetheusToPrometheus), true));
            lightingStage = LightingStage.EpimetheusToPrometheus;
            prometheusRoom2.enabled = true;
        }

        if (!prometheusRoom2.enabled && lightingStage == LightingStage.EpimetheusToPrometheus)
        {
            TriggerLightsSegmented(GetFirstChildren(PrometheusToPallas), true);
            TriggerLightsSegmented(GetFirstChildren(PallasToEpimetheus), true);
            TriggerLightsSegmented(GetFirstChildren(EpimetheusToPrometheus), true);
            TriggerLights(TheRest.GetComponentsInChildren<MeshRenderer>(), true);
            TriggerLights(EntryLights.GetComponentsInChildren<MeshRenderer>(), true);
            PallasNearDoor.ModifyLarcenyReq(0);
            ExteriorDoor.ModifyLarcenyReq(0);
            lightingStage = LightingStage.End;
        }
    }

    private void TriggerLights(MeshRenderer[] lights, bool turnLightsOn)
    {
        foreach (MeshRenderer light in lights)
        {
            Material[] lightMaterials = light.materials;
            lightMaterials[1] = turnLightsOn ? emissiveMaterial : nonEmissiveMaterial; ;
            light.materials = lightMaterials;

            Light areaLight = light.GetComponentInChildren<Light>();
            if (areaLight)
            {
                areaLight.enabled = turnLightsOn;
            }
        }
    }

    private void TriggerLightsSegmented(Transform[] lightSegments, bool turnLightsOn)
    {
        foreach (Transform lightSegment in lightSegments)
        {
            foreach (MeshRenderer light in lightSegment.GetComponentsInChildren<MeshRenderer>())
            {
                Material[] lightMaterials = light.materials;
                lightMaterials[1] = turnLightsOn ? emissiveMaterial : nonEmissiveMaterial; ;
                light.materials = lightMaterials;

                Light areaLight = light.GetComponentInChildren<Light>();
                if (areaLight)
                {
                    areaLight.enabled = turnLightsOn;
                }
            }
        }
    }

    private IEnumerator TriggerLightsCoroutine(Transform[] lightSegments, bool turnLightsOn)
    {
        foreach (Transform lightSegment in lightSegments) 
        {
            foreach (MeshRenderer light in lightSegment.GetComponentsInChildren<MeshRenderer>())
            {
                Material[] lightMaterials = light.materials;
                lightMaterials[1] = turnLightsOn ? emissiveMaterial : nonEmissiveMaterial; ;
                light.materials = lightMaterials;

                Light areaLight = light.GetComponentInChildren<Light>();
                if (areaLight)
                {
                    areaLight.enabled = turnLightsOn;
                }
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    private Transform[] GetFirstChildren(Transform parent)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        Transform[] firstChildren = new Transform[parent.childCount];
        int index = 0;
        foreach (Transform child in children)
        {
            if (child.parent == parent)
            {
                firstChildren[index] = child;
                index++;
            }
        }
        return firstChildren;
    }
}
