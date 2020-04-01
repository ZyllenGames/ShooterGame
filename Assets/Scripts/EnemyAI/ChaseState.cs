using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
    Transform m_TargetTransfrom;
    float m_TimeInterCheck = 0.25f;
    float m_CurTime;
    float m_StartAttackDist;
    float m_QuitChaseDist;


    public System.Action OutOfChaseRange;
    public System.Action EnterAttackRange;

    public ChaseState(GameObject owner, Transform targetTransfrom, float startAttackDist, float quitChaseDist, System.Action outOfChaseRange, System.Action enterAttackRange) : base(owner)
    {
        m_TargetTransfrom = targetTransfrom;
        m_StartAttackDist = startAttackDist;
        m_QuitChaseDist = quitChaseDist;
        OutOfChaseRange = outOfChaseRange;
        EnterAttackRange = enterAttackRange;
    }

    public override void EnterState()
    {
        m_CurTime = m_TimeInterCheck;
        NavMeshAgent navAgent = OwnerObject.GetComponent<NavMeshAgent>();
        navAgent.enabled = true;
    }

    public override void UpdateState(float sqrdist)
    {
        m_CurTime += Time.deltaTime;

        if(m_CurTime > m_TimeInterCheck)
        {
            m_CurTime = 0f;
            if(OwnerObject!= null)
            {
                NavMeshAgent navAgent = OwnerObject.GetComponent<NavMeshAgent>();
                if(navAgent != null)
                    navAgent.SetDestination(m_TargetTransfrom.position);
            }
            
        }
        if (sqrdist > Mathf.Pow(m_QuitChaseDist, 2))
            OutOfChaseRange();
        if (sqrdist < Mathf.Pow(m_StartAttackDist, 2))
            EnterAttackRange();
    }

    public override void ExitState()
    {
        NavMeshAgent navAgent = OwnerObject.GetComponent<NavMeshAgent>();
        navAgent.enabled = false;
    }
}
