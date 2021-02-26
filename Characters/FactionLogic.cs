using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionLogic : MonoBehaviour
{
    private Transform[] Tranform_Holder = new Transform[STARTUP_DECLARATIONS.FactionCount];
    private float[][] ReputationMatrix = new float[STARTUP_DECLARATIONS.FactionCount][]; //No entry for player
    //Goes from 0 to 2000
    //Under 200 means they are your enemy
    //Over 1800 means they are your ally

    public bool ReturnIsEnemy(FactionsEnum Fac_caster, FactionsEnum Fac_target)
    {
        return AccessReputationMatrix((int)Fac_caster, (int)Fac_target) <= STARTUP_DECLARATIONS.EnemyNumber;
    }

    public bool ReturnIsEnemyCustomPlayer(FactionsEnum Fac_caster, int CustomMod) //Allows some characters to have different reputations with the player
    {
        return AccessReputationMatrix((int)Fac_caster, 0) + CustomMod <= STARTUP_DECLARATIONS.EnemyNumber;
    }

    public bool ReturnIsAlly(FactionsEnum Fac_caster, FactionsEnum Fac_target)
    {
        return AccessReputationMatrix((int)Fac_caster, (int)Fac_target) >= STARTUP_DECLARATIONS.AllyNumber;
    }

    public void RallyAllies(FactionsEnum fac_in, Transform thisTransform, int CustomRep, float EnableRange = 100)
    {
        for (int i = 1; i < STARTUP_DECLARATIONS.FactionCount; ++i) //Don't Include Player; That is above
        {
            if (ReturnIsAlly(fac_in, (FactionsEnum)i))
            {
                foreach (Transform trans in Tranform_Holder[i])
                {
                    if (trans.gameObject.activeSelf && (trans != thisTransform)) //enabled
                    {
                        float dist = (trans.position - thisTransform.position).magnitude;
                        if (dist < EnableRange) //Need to check because rogues will attack all
                        {
                            trans.GetComponentInChildren<EnemyTemplateMaster>().Set_customReputation(CustomRep);
                            trans.GetComponentInChildren<EnemyTemplateMaster>().EnableAI(false);
                        }
                    }
                }
            }
        }
    }

    public Transform FindEnemy(FactionsEnum fac_in, Transform thisTransform, int CustomMod, float FindRange = 100)
    {
        Transform ClosestEnemy = null;
        float min_dist = FindRange;

        if (ReturnIsEnemyCustomPlayer(fac_in, CustomMod))
        {
            Transform trans = GameObject.Find("Player").transform;
            float dist = (trans.position - thisTransform.position).magnitude;
            if (dist < min_dist)
            {
                min_dist = dist;
                ClosestEnemy = trans;
            }
        }

        for (int i = 1; i < STARTUP_DECLARATIONS.FactionCount; ++i) //Don't Include Player; That is above
        {
            if (ReturnIsEnemy(fac_in, (FactionsEnum)i))
            {
                foreach (Transform trans in Tranform_Holder[i])
                {
                    if (trans.gameObject.activeSelf && (trans != thisTransform)) //enabled
                    {
                        float dist = (trans.position - thisTransform.position).magnitude;
                        if (dist < min_dist) //Need to check because rogues will attack all
                        {
                            min_dist = dist;
                            ClosestEnemy = trans;
                        }
                    }
                }
            }
        }
        return ClosestEnemy;
    }

    private void Awake()
    {
        Tranform_Holder[0] = null;
        for (int i = 1; i < STARTUP_DECLARATIONS.FactionCount; ++i)
        {
            Tranform_Holder[i] = transform.Find(STARTUP_DECLARATIONS.FactionsEnumReverse[i]);
        }

        SetupReputationMatrix();
    }

    private float AccessReputationMatrix(int A, int B)
    {
        if (A > B)
        {
            int C = A;
            A = B;
            B = C;
        }

        B -= A;

        return ReputationMatrix[A][B];
    }


    private void SetupReputationMatrix()
    {
        ReputationMatrix[(int)FactionsEnum.Player] = new float[STARTUP_DECLARATIONS.FactionCount]
        {
            2000, 0, 0, 0, 1000, 1000, 1000, 1000
        };
        ReputationMatrix[(int)FactionsEnum.Rogue] = new float[STARTUP_DECLARATIONS.FactionCount - 1]
        {
            0, 0, 0, 0, 0, 0, 0
        };
        ReputationMatrix[(int)FactionsEnum.Feral] = new float[STARTUP_DECLARATIONS.FactionCount - 2]
        {
            2000, 0, 0, 0, 0, 0
        };
        ReputationMatrix[(int)FactionsEnum.AntiPlayer] = new float[STARTUP_DECLARATIONS.FactionCount - 3]
        {
            2000, 1000, 1000, 1000, 1000
        };
        ReputationMatrix[(int)FactionsEnum.B] = new float[STARTUP_DECLARATIONS.FactionCount - 4]
        {
            2000, 1000, 1000, 1000
        };
        ReputationMatrix[(int)FactionsEnum.C] = new float[STARTUP_DECLARATIONS.FactionCount - 5]
        {
            2000, 1000, 1000
        };
        ReputationMatrix[(int)FactionsEnum.Plantation] = new float[STARTUP_DECLARATIONS.FactionCount - 6]
        {
            2000, 1000
        };
        ReputationMatrix[(int)FactionsEnum.MidwayCityCivilian] = new float[STARTUP_DECLARATIONS.FactionCount - 7]
        {
            2000
        };
    }

}
