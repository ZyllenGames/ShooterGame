using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    Transform m_Target;
    float m_CurR;
    float m_TarR;
    float m_QuitAttackDist;
    float m_AttackDist;
    float m_AttackMovingSpeed = 3.5f;

    public System.Action OutOfAttackRange;

    public AttackState(GameObject owner, Transform target, System.Action outOfAttackRange, float quitAttackDist, float attackDist) : base(owner)
    {
        m_Target = target;
        OutOfAttackRange = outOfAttackRange;
        m_QuitAttackDist = quitAttackDist;
        m_AttackDist = attackDist;
    }

    public override void EnterState()
    {
        OwnerObject.GetComponent<GunController>().SetTarget(m_Target);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState(float sqrdist)
    {
        if(m_Target != null)
        {
            m_TarR = (m_Target.position - OwnerObject.transform.position).CalculateDirAngleY();
            m_CurR = Mathf.Lerp(m_CurR, m_TarR, 0.1f);
            OwnerObject.transform.rotation = Quaternion.Euler(Vector3.up * m_CurR);
            OwnerObject.GetComponent<GunController>().AIShot();
            if (sqrdist > Mathf.Pow(m_AttackDist, 2))
                OwnerObject.transform.position = Vector3.MoveTowards(OwnerObject.transform.position, m_Target.position, m_AttackMovingSpeed * Time.deltaTime);
            if (sqrdist > Mathf.Pow(m_QuitAttackDist, 2))
                OutOfAttackRange();
        }
        
        
    }
}
