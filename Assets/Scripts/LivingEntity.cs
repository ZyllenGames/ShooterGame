using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float StartHealth;
    protected bool m_IsDead;
    protected float m_Health;

    public GameObject DeathEffect;
    public string DeathSFX;

    protected virtual void Awake()
    {
        m_Health = StartHealth;
        m_IsDead = false;
    }

    public virtual void onHit(float damage, Vector3 hitPos, Vector3 hitDir)
    {
        m_Health -= damage;
        if (m_Health <= 0f && !m_IsDead)
            Die(hitPos, hitDir);
    }
   

    public virtual void Die(Vector3 hitPos, Vector3 hitDir)
    {
        m_IsDead = true;

        if(DeathEffect != null)
        {
            GameObject deadeffect = Instantiate(DeathEffect, hitPos, Quaternion.FromToRotation(Vector3.forward, hitDir));
            float lifetime = deadeffect.GetComponent<ParticleSystem>().main.startLifetime.constant;
            Destroy(deadeffect, lifetime);

        }

        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(DeathSFX, transform.position);

        Destroy(gameObject);
    }

}
