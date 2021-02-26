using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMaster : MonoBehaviour
{
    private Collider rollingCollider;
    private PlayerMovement playerMovement;
    private PlayerAnimationUpdater animationUpdater;
    private WeaponController weaponController;
    private AbilitiesController abilitiesController;
    private ConsumableController consumableController;
    private UIController UIControl;

    public static InputManager inputActions;


    private void Awake()
    {
        inputActions = new InputManager();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Core functionality
        playerMovement = GetComponent<PlayerMovement>();
        weaponController = GetComponentInChildren<WeaponController>();
        animationUpdater = GetComponentInChildren<PlayerAnimationUpdater>();
        abilitiesController = GetComponentInChildren<AbilitiesController>();
        consumableController = GetComponentInChildren<ConsumableController>();
        UIControl = GameObject.Find("UI").GetComponent<UIController>();
        rollingCollider = GameObject.Find("RollingCollider").GetComponent<Collider>();
    }

    private void Update() //THE ORDER OF THESE MATTER, UpdatePlayerState needs to be before handle weapon and handleabilites so that a roll can start
    {
        if (!UIControl.GamePaused)
        {
            // Player movement
            playerMovement.UpdatePlayerState(); //must be above abilites and weapons

            //Ability Controller
            abilitiesController.HandleAbilities(1f);

            // Item Controller
            weaponController.HandleWeapon();

            //Consumable Controller
            consumableController.HandleConsumables();

            // Player animation
            animationUpdater.UpdateAnimation();
        }
        else if (UIControl.inPauseMenu)
        {
            playerMovement.PauseAimTowardCamera();
            weaponController.PauseMenuWeaponControl();
            animationUpdater.UpdateAnimationPauseMenu();
        }
        else if (UIControl.inDialogueMenu)
        {
            if (!UIControl.npcTransform)
            {
                return;
            }

            playerMovement.DialogueMenuAimTowards(UIControl.npcTransform);
            Vector3 direction = (transform.position - UIControl.npcTransform.position);
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            UIControl.npcTransform.rotation = Quaternion.RotateTowards(UIControl.npcTransform.rotation, Quaternion.Euler(new Vector3(0, angle, 0)), 270 * Time.unscaledDeltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (!UIControl.GamePaused)
        {
            playerMovement.Move();
            Vector2 movementInput = inputActions.Movement.Move.ReadValue<Vector2>();
        }

        rollingCollider.enabled = playerMovement.GetMoveState() == MoveState.Rolling;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
