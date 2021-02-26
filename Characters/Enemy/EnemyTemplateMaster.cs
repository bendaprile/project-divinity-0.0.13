using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

public class EnemyTemplateMaster : MonoBehaviour
{
    [SerializeField] protected FactionsEnum factionEnum;
    [SerializeField] private string EnemyTypeName;
    [SerializeField] private int exp_reward = 100;
    [SerializeField] protected float destroyAfterDeathDelay = 60f;
    [SerializeField] protected bool stunable = true;
    [SerializeField] protected bool knockbackable = true;
    [SerializeField] protected float MaxMovementForce;
    [SerializeField] protected float NewEnemyAcquireTime = 1f;

    [SerializeField] protected float DamageRepMod = -4000;
    [SerializeField] protected float expDamageThreshold = .5f;

    [SerializeField] protected int customReputation = 0;

    protected bool AIenabled;
    protected bool NormalMovement = true;

    public GameObject player;
    protected Transform Current_Target;

    protected FactionLogic facLogic;
    protected QuestsHolder QH;
    protected Rigidbody rB;
    protected EnemyAnimationUpdater animationUpdater;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Health health;
    protected Transform Hitbox;
    protected EnemyHealthBar EHealBar;

    // Roam logic
    protected float roam_speed = 0;
    protected bool roam = false;
    protected Transform Spawner_Transform = null;
    protected float roam_rangeX = 0;
    protected float roam_rangeZ = 0;
    protected float roam_cd = 0;
    protected float cd_randomness = 0;
    protected float roam_time_tracker = 0;
    // Roam logic

    protected float original_speed;
    protected float PlayerPercentDamage = 0f;

    protected bool cc_immune;
    protected float NewTarget = 0f;
    protected float StunRelease;
    protected float timer;
    protected Transform deadEnemyParent;

    protected Vector3 direction; //Used by multiple funcs


    //anchor logic
    private Transform NPCHolder;
    private Vector3 anchor;
    private float anchorTime;
    private Transform anchorTerrain;
    //anchor logic

    ////////////////////////////////////Access Vars
    public string Return_EnemyTypeName()
    {
        return EnemyTypeName;
    }
    public void Set_exp_reward(int xp_in)
    {
        exp_reward = xp_in;
    }
    public int Return_customReputation()
    {
        return customReputation;
    }
    public void Set_customReputation(int customRep_in)
    {
        customReputation = customRep_in;
    }

    public void PlayerDamageTaken(float fractionTaken)
    {
        customReputation += (int)(DamageRepMod * fractionTaken);
        PlayerPercentDamage += fractionTaken;
    }
    public FactionsEnum Return_FactionEnum()
    {
        return factionEnum;
    }
    public Transform Return_Current_Target()
    {
        return Current_Target;
    }
    ////////////////////////////////////Access Vars


    ////////////////////////////////////Public Usable Functions
    public virtual void SpawnEnemy(FactionsEnum fac, bool Roam_in, Transform Spawner_Transform_in, float Roam_RangeX_in, float Roam_RangeZ_in, float Roam_cd_in, float cd_randomness_in, float roam_speed_in)
    {
        factionEnum = fac;
        roam = Roam_in;
        Spawner_Transform = Spawner_Transform_in;
        roam_rangeX = Roam_RangeX_in;
        roam_rangeZ = Roam_RangeZ_in;
        roam_cd = Roam_cd_in;
        cd_randomness = cd_randomness_in;
        roam_speed = roam_speed_in;
    }

    public virtual void EnableAI(bool RallyAllies) //Do if damaged or an enemy comes into range... DO NOT PROPAGATE
    {
        if (Hitbox.tag == "DeadEnemy" || AIenabled)
        {
            return;
        }

        if (RallyAllies)
        {
            facLogic.RallyAllies(factionEnum, transform, customReputation);
        }

        AIenabled = true;
        agent.speed = original_speed;
        EHealBar.Enable();
    }

    public virtual void EnemyKnockback(Vector3 Force)
    {
        if (cc_immune || !knockbackable)
        {
            return;
        }
        animationUpdater.PlayBlockingAnimation("take_damage", true); //Knockback duration determined by animation length
        rB.velocity = new Vector3(0f, 0f, 0f);
        rB.AddForce(Force);
    }

    public virtual void EnemyStun(float duration)
    {
        if (cc_immune || !stunable)
        {
            return;
        }
        StunRelease = timer + duration;
    }

    public virtual void Death()
    {
        if(Hitbox.tag != "DeadEnemy") //Call only once
        {
            if (PlayerPercentDamage >= expDamageThreshold)
            {
                player.GetComponent<PlayerStats>().AddEXP(exp_reward);
                QH.CheckGenericKillObjectives(transform.gameObject, new Vector2(transform.position.x, transform.position.y));
            }

            if (Spawner_Transform)
            {
                Spawner_Transform.GetComponent<Spawner>().ChildDied();
            }

            // Freeze the position and remove collision of the enemy
            rB.constraints = RigidbodyConstraints.FreezeAll;
            rB.angularVelocity = Vector3.zero;
            animationUpdater.PlayAnimation("death", false, true);
            EHealBar.Disable();

            // Makes it so the dead enemy cannot be targeted like an alive one
            Hitbox.tag = "DeadEnemy";
            agent.enabled = false;
            Hitbox.GetComponent<Collider>().enabled = false;

            transform.parent = deadEnemyParent;
            Destroy(gameObject, destroyAfterDeathDelay);
        }
    }
    ////////////////////////////////////Public Usable Functions


    ////////////////////////////////////Protected Virtual Functions
    protected virtual void Start()
    {
        cc_immune = false;
        timer = 0f;
        player = GameObject.Find("Player");
        Hitbox = transform.Find("Hitbox");
        deadEnemyParent = GameObject.Find("DeadNPC").transform;

        AIenabled = false;
        QH = GameObject.Find("QuestsHolder").GetComponent<QuestsHolder>();
        rB = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        animationUpdater = GetComponentInChildren<EnemyAnimationUpdater>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        health = GetComponentInChildren<Health>();
        EHealBar = GetComponentInChildren<EnemyHealthBar>();
        facLogic = GameObject.Find("NPCs").GetComponent<FactionLogic>();

        agent.enabled = true;
        original_speed = agent.speed;
    }

    protected virtual void AIFunc()
    {
        agent.SetDestination(Current_Target.position);
    }

    protected virtual void RoamingFunc()
    {
        agent.speed = roam_speed;
        if (roam_time_tracker <= 0)
        {
            roam_time_tracker = roam_cd + Random.Range(-cd_randomness, cd_randomness); ;
            Vector3 roam_dest = Spawner_Transform.position;
            roam_dest.x += Random.Range(-roam_rangeX, roam_rangeX);
            roam_dest.z += Random.Range(-roam_rangeZ, roam_rangeZ);
            agent.SetDestination(roam_dest);
        }
        else
        {
            roam_time_tracker -= Time.fixedDeltaTime;
        }
    }

    protected virtual void EnemyBasicAnimation()
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, angle, 0)), 180 * Time.fixedDeltaTime);
        if(rB.velocity.magnitude < 0.1f)
        {
            animationUpdater.PlayAnimation("idle");
        }
        else
        {
            animationUpdater.PlayAnimation("Blend Tree");
        }
    }

    protected virtual bool CustomFixedFunc()
    {
        return false;
    }

    protected virtual void AIDisableFunc()
    {

    }
    ////////////////////////////////////Protected Virtual Functions


    ////////////////////////////////////Protected Fixed Functions
    protected void FixedUpdate()
    {
        if (Hitbox.tag == "DeadEnemy")
        {
            return;
        }

        if(timer > anchorTime) //Disable logic
        {
            if(transform.position.y < -10)
            {
                transform.position = anchor;
                transform.parent = anchorTerrain;
            }
            else
            {
                anchorTime = timer + 10f;

                RaycastHit hit;
                Ray ray = new Ray(transform.position + (2 * transform.up), -transform.up);
                if (Physics.Raycast(ray, out hit, 10f, 1 << LayerMask.NameToLayer("Terrain"))) //If terrain found below
                {
                    anchor = transform.position;
                    anchorTerrain = hit.transform;
                }
            }
        }

        animator.SetFloat("MoveSpeed", rB.velocity.magnitude); //Can be overwritten by humanoids in CustomFixedFunc... MUST BE ABOVE
        bool breakVar = CustomFixedFunc();
        if (breakVar)
        {
            return;
        }

        timer += Time.fixedDeltaTime;



        if (AIenabled)
        {
            if (!Current_Target || timer >= NewTarget)
            {
                Current_Target = facLogic.FindEnemy(factionEnum, transform, customReputation); //TODO Use collider prob
                if (Current_Target)
                {
                    NewTarget = timer + NewEnemyAcquireTime;
                    Current_Target = Current_Target.Find("Hitbox");
                }
            }

            if (!Current_Target) //DO NOT MAKE AN IF/ELSE, because the current_target can be set null above
            {
                AIenabled = false;
                AIDisableFunc();
            }
        }

        if (animationUpdater.ReturnBlocked() || StunRelease > timer) //knockback, stun, or self-disable
        {
            return;
        }
        else if (AIenabled)
        {
            AIFunc();
        }
        else if (roam)
        {
            RoamingFunc();
        }

        if (NormalMovement)
        {
            direction = agent.nextPosition - transform.position;
            EnemyMovement();
            EnemyBasicAnimation();
        }
    }

    protected void EnemyMovement()
    {
        if (direction.magnitude > 1)
        {
            agent.nextPosition = (transform.position + agent.nextPosition) / 2;
        }

        direction = direction.normalized;

        float final_dist = new Vector2(transform.position.x - agent.destination.x, transform.position.z - agent.destination.z).magnitude;
        float temp_speed;
        if (final_dist > 1)
        {
            temp_speed = agent.speed;
        }
        else
        {
            temp_speed = agent.speed * final_dist / 1; //Slowdown near dest
        }

        direction = (direction * temp_speed - rB.velocity);

        Vector3 currentforce = new Vector3();
        if (direction.magnitude > 1f)
        {
            direction = direction.normalized;
            currentforce = MaxMovementForce * direction;
        }
        else
        {
            currentforce = MaxMovementForce * direction;
        }

        rB.AddForce(new Vector3(currentforce.x, 0, currentforce.z));
    } //This will always follow the agent
      ////////////////////////////////////Protected Fixed Functions

    private void OnEnable()
    {
        if (!NPCHolder)
        {
            string FactionName = STARTUP_DECLARATIONS.FactionsEnumReverse[(int)factionEnum];
            NPCHolder = GameObject.Find(FactionName).transform;
        }
        anchor = transform.position;
        anchorTerrain = transform.parent; //might not be terrian, but this prob is fine

        transform.parent = NPCHolder;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!AIenabled)
        {
            if (other.tag == "Player")
            {
                if (facLogic.ReturnIsEnemyCustomPlayer(factionEnum, customReputation))
                {
                    Current_Target = other.transform;
                    EnableAI(true);
                }
            }
            else if (other.tag == "BasicEnemy")
            {
                if (facLogic.ReturnIsEnemy(factionEnum, other.GetComponentInParent<EnemyTemplateMaster>().Return_FactionEnum()))
                {
                    Current_Target = other.transform;
                    EnableAI(true);
                }
            }
        }
    }

    public virtual void AnimationCalledFunc0()
    {

    }

    public virtual void AnimationCalledFunc1()
    {

    }
}
