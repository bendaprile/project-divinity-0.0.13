using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIController : MonoBehaviour
{
    public GameObject Menu;
    public GameObject HUD;
    public GameObject Map;
    public GameObject InteractiveObjectMenu;
    public GameObject LevelUpMenu;
    public GameObject DialogueMenu;
    public CinemachineBrain cinemachineBrain;
    [SerializeField] private Transform EXPUI;

    public bool GamePaused;
    public bool inPauseMenu = false;
    public bool inDialogueMenu = false;
    public Transform npcTransform;

    private bool inescapableExternalMenu;
    private bool ExternalMenuOpen;
    private float originalTimeScale;
    private bool mapOpen = false;
    private PlayerStats playerStats;
    private PlayerAnimationUpdater playerAnimationUpdater;
    private CameraStateController camStateController;
    private Transform UIparent;
    private Transform originalParent;

    // Start is called before the first frame update
    void Start()
    {
        inescapableExternalMenu = false;
        ExternalMenuOpen = false;
        Menu.SetActive(false);
        originalTimeScale = Time.timeScale;
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerAnimationUpdater = playerStats.GetComponentInChildren<PlayerAnimationUpdater>();
        UIparent = transform.Find("UIParent");
        camStateController = FindObjectOfType<CameraStateController>();
        originalParent = EXPUI.parent;
    }

    // Update is called once per frame
    void Update()
    {

        // TODO: Handle Input through InputManager and not direct key references
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(inescapableExternalMenu)
            {
                //Do Nothing
            }
            else if (ExternalMenuOpen)
            {
                ExternalMenuOpen = false;
                InteractiveObjectMenu.GetComponent<InteractiveObjectMenuUI>().DisablePanel();
                Unpaused(); //NO CAMERA MOVEMENT
                UnpauseEXPBar();
            }
            else
            {
                Menu.SetActive(!Menu.activeSelf);
                if (Menu.activeSelf == true)
                {
                    Paused(true);
                    PauseEXPBar();
                }
                else
                {
                    Unpaused(true);
                    UnpauseEXPBar();
                }
            }
        }

        // TODO: Handle Input through InputManager and not direct key references
        if (!inescapableExternalMenu && !ExternalMenuOpen && Input.GetKeyDown(KeyCode.Tab))
        {
            if (Menu.activeSelf)
            {
                Menu.GetComponent<MenuController>().WorldEnable();                
            }
            else
            {
                EnableDisableMap(!mapOpen);
            }
        }
    }

    public void EnableDisableMap(bool enable)
    {
        if (enable)
        {
            Map.SetActive(true);
            Map.GetComponentInChildren<Animator>().Play("In");
            mapOpen = true;
        }
        else if (Map.activeSelf)
        {
            mapOpen = false;
            Map.GetComponentInChildren<Animator>().Play("Out");
            StartCoroutine(TurnOffPanel(Map));
        }
    }

    public void OpenInteractiveMenu(GameObject container)
    {
        Paused();
        Map.SetActive(false);
        ExternalMenuOpen = true;
        InteractiveObjectMenu.SetActive(true);
        InteractiveObjectMenu.GetComponent<InteractiveObjectMenuUI>().Refresh(container);
    }

    public void LevelUpMenuBool(bool open)
    {
        if (open)
        {
            Paused();
            Map.SetActive(false);
            inescapableExternalMenu = true;
            LevelUpMenu.SetActive(true);
            LevelUpMenu.GetComponent<Animator>().Play("Modal Window In");
        }
        else
        {
            Unpaused();
            inescapableExternalMenu = false;
            LevelUpMenu.GetComponent<Animator>().Play("Modal Window Out");
            StartCoroutine(TurnOffPanel(LevelUpMenu));
        }
    }

    public void ReturnMapLocation(Transform mapRect)
    {
        StartCoroutine(ReturnMapLocationCoroutine(mapRect));
    }

    private IEnumerator TurnOffPanel(GameObject panel)
    {
        yield return new WaitForSecondsRealtime(0.3f);

        if (!Map || !mapOpen)
        {
            panel.SetActive(false);
        }
    }

    private IEnumerator ReturnMapLocationCoroutine(Transform mapRect)
    {
        yield return new WaitForSecondsRealtime(0.3f);

        if (!mapOpen)
        {
            mapRect.localPosition = new Vector3(960f, 597.5f);
        }
    }

    public void DialogueMenuBool(Transform DiaData = null)
    {
        if (DiaData != null)
        {
            Paused();
            cinemachineBrain.m_IgnoreTimeScale = true;
            cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
            playerAnimationUpdater.SetUpdateMode(AnimatorUpdateMode.UnscaledTime);
            playerAnimationUpdater.PlayAnimation("idle");

            Map.SetActive(false);
            inescapableExternalMenu = true;
            DialogueMenu.SetActive(true);
            DialogueMenu.GetComponent<Animator>().Play("Panel In");
            DialogueMenu.GetComponent<DiaParent>().SetupDia(DiaData);

            HumanoidMaster TempMaster = DiaData.GetComponentInParent<HumanoidMaster>();
            if(TempMaster) //Don't set if no npc
            {
                npcTransform = TempMaster.transform;
            }
            inDialogueMenu = true;

            List<Transform> targetTransforms = new List<Transform>();
            targetTransforms.Add(playerAnimationUpdater.GetComponentInParent<PlayerMaster>().transform);
            targetTransforms.Add(TempMaster.transform);
            camStateController.SetDiaTargetGroup(targetTransforms);
            camStateController.SetCamState(CameraStateController.CameraState.DialogueMenuCam);
        }
        else
        {
            Unpaused(true);
            inescapableExternalMenu = false;
            DialogueMenu.GetComponent<Animator>().Play("Panel Out");
            camStateController.RevertCamToPreviousState();
            inDialogueMenu = false;
            StartCoroutine(TurnOffPanel(DialogueMenu));
        }
    }

    void Paused(bool zoomOnCharacter = false)
    {
        if (zoomOnCharacter)
        {
            cinemachineBrain.m_IgnoreTimeScale = true;
            cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
            playerAnimationUpdater.SetUpdateMode(AnimatorUpdateMode.UnscaledTime);
            playerAnimationUpdater.PlayAnimation("idle");
            camStateController.SetCamState(CameraStateController.CameraState.PauseMenuCam);
            inPauseMenu = true; 
        }

        playerAnimationUpdater.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        playerAnimationUpdater.GetComponentInParent<Rigidbody>().angularVelocity = Vector3.zero;
        Time.timeScale = 0f;
        GamePaused = true;
        HUD.GetComponent<CanvasGroup>().alpha = 0;
    }

    void Unpaused(bool zoomOnCharacter = false)
    {
        if (zoomOnCharacter)
        {
            cinemachineBrain.m_IgnoreTimeScale = false;
            cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
            playerAnimationUpdater.SetUpdateMode(AnimatorUpdateMode.Normal);
            playerAnimationUpdater.PlayAnimation("idle");
            camStateController.RevertCamToPreviousState();
            inPauseMenu = false;
        }

        Time.timeScale = originalTimeScale;
        GamePaused = false;
        HUD.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void PauseEXPBar()
    {
        EXPUI.gameObject.SetActive(true);
        EXPUI.SetParent(UIparent);
        EXPUI.transform.position = UIparent.transform.position;
        //EXPUI.transform.localScale = new Vector3(1f, 1f, 1f);
        EXPUI.Find("EXPNum").gameObject.SetActive(false);
    }

    private void UnpauseEXPBar()
    {
        EXPUI.gameObject.SetActive(false);
        EXPUI.SetParent(originalParent);
        EXPUI.transform.position = originalParent.transform.position;
        //EXPUI.transform.localScale = new Vector3(1f, 1f, 1f);
        EXPUI.Find("EXPNum").gameObject.SetActive(true);
    }
}
