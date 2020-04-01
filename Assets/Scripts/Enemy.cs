using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    NavMeshAgent m_NavMeshAgent;
    Transform m_TargetTransform;

    float m_DistanceInitAttack;
    float m_NextAttackTime;
    float m_TimeBetweenAttack = 2f;
    public float AttackStrength = 5f;

    Material m_Material;
    Color m_OriginColor;

    public static event System.Action OnKilledStatic;

    protected override void Awake()
    {
        base.Awake();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj!= null)
            m_TargetTransform = playerObj.transform;
        m_Material = GetComponent<Renderer>().material;
        m_OriginColor = m_Material.color;

        m_DistanceInitAttack = 1f;
    }

    void Start()
    {
        StartCoroutine(FollowTarget());
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Time.time > m_NextAttackTime)
        {
            if(m_TargetTransform != null)
            {
                float dissqr = (transform.position - m_TargetTransform.position).sqrMagnitude;
                if (dissqr < Mathf.Pow(m_DistanceInitAttack, 2))
                {
                    m_NextAttackTime = Time.time + m_TimeBetweenAttack;
                    IDamageable damageobj = m_TargetTransform.GetComponent<IDamageable>();
                    print("Player been hit!");
                    m_Material.color = Color.red;
                    if (damageobj != null)
                        damageobj.onHit(AttackStrength, Vector3.one, Vector3.one);

                    if(AudioManager.Instance != null)
                        AudioManager.Instance.PlaySound("EnemyAttack", transform.position);
                }
                else
                    m_Material.color = m_OriginColor;
            }

        }

    }

    IEnumerator FollowTarget()
    {
        float timeinterval = 0.25f;

        while(true)
        {
            if(m_TargetTransform != null)
            {
                Vector3 target = new Vector3(m_TargetTransform.position.x, 1f, m_TargetTransform.position.z);
                if(!m_IsDead)
                    m_NavMeshAgent.SetDestination(target);
            }
            yield return new WaitForSeconds(timeinterval);
        }
    }


    public override void Die(Vector3 hitPos, Vector3 hitDir)
    {
        base.Die(hitPos, hitDir);
        if (OnKilledStatic != null)
        {
            OnKilledStatic();
            if(AudioManager.Instance != null)
                AudioManager.Instance.PlaySound("EnemyDeath", transform.position);
        }
    }
}
