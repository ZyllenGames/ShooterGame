using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : LivingEntity
{
    StateMachine m_StateMachine;
    public float WanderSpeed = 3f;
    Transform Target;
    public float StartChasingDistance = 50f;
    public float StartAttackDistance = 30f;
    public float AttackDistance = 20f;
    public float QuitAttackDistance = 60f;
    public float QuitChasingDistance = 80f;

    WanderState m_WanderState;
    ChaseState m_ChaseState;
    AttackState m_AttackeState;

    public static event System.Action OnKilledStatic;

    protected override void Awake()
    {
        base.Awake();
        Target = FindObjectOfType<Player>().transform;
        m_StateMachine = GetComponent<StateMachine>();
        m_WanderState = new WanderState(gameObject, WanderSpeed, StartChasingDistance, ChangeToChaseState);
        m_ChaseState = new ChaseState(gameObject, Target, StartAttackDistance, QuitChasingDistance, ChangeToWanderState, ChangeToAttackState);
        m_AttackeState = new AttackState(gameObject, Target, ChangeToChaseState, QuitAttackDistance, AttackDistance);
    }

    void Start()
    {
        m_StateMachine.ChangeState(m_WanderState);
    }

    void Update()
    {
        float sqrdist = 0;
        if (Target != null)
            sqrdist = (Target.position - transform.position).sqrMagnitude;
       

        m_StateMachine.StateUpdate(sqrdist);
    }

    void ChangeToWanderState()
    {
        m_StateMachine.ChangeState(m_WanderState);
    }

    void ChangeToChaseState()
    {
        m_StateMachine.ChangeState(m_ChaseState);
    }

    void ChangeToAttackState()
    {
        m_StateMachine.ChangeState(m_AttackeState);
    }
    public override void Die(Vector3 hitPos, Vector3 hitDir)
    {
        base.Die(hitPos, hitDir);
        if (OnKilledStatic != null)
        {
            OnKilledStatic();
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySound("EnemyDeath", transform.position);
        }
    }
}
