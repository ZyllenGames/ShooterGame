using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : IState
{
    Vector3 m_Target;
    float m_Speed;
    float m_CurR;
    float m_TarR;
    float m_StartChaseDist;

    public System.Action StartChase;

    public WanderState(GameObject owner, float speed, float startChaseDist, System.Action startChase) : base(owner)
    {
        m_Speed = speed;
        m_StartChaseDist = startChaseDist;
        StartChase = startChase;
    }

    public override void EnterState()
    {
        TargetUpdate();
        m_CurR = 0f;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState(float sqrdist)
    {
        if (Vector3.Distance(OwnerObject.transform.position, m_Target) > 1f)
        {
            m_CurR = Mathf.Lerp(m_CurR, m_TarR, 0.1f);
            OwnerObject.transform.rotation = Quaternion.Euler(Vector3.up * m_CurR);
            OwnerObject.transform.position = Vector3.MoveTowards(OwnerObject.transform.position, m_Target, m_Speed * Time.deltaTime);
        }
        else
            TargetUpdate();
        if (sqrdist < Mathf.Pow(m_StartChaseDist, 2))
            StartChase();
    }

    void TargetUpdate()
    {
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        dir.Normalize();
        m_Target = OwnerObject.transform.position + dir * Random.Range(3, 5);
        m_TarR = (m_Target - OwnerObject.transform.position).CalculateDirAngleY();
    }
}
