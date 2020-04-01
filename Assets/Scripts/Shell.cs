using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    float m_Force;
    float m_Torque;

    float m_LifeTime;
    Material m_Material;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Force = 200f;
        m_Torque = 500f;
        m_LifeTime = 3f;
        m_Material = GetComponent<Renderer>().material;
    }

    void Start()
    {
        m_Rigidbody.AddForce(transform.up * m_Force);
        m_Rigidbody.AddTorque(Random.insideUnitSphere * m_Torque);

        StartCoroutine(Fade());
        Destroy(gameObject, m_LifeTime);
    }

    IEnumerator Fade()
    {
        float startfadetime = 1.5f;
        float curtime = 0f;
        Color target = new Color(m_Material.color.r, m_Material.color.g, m_Material.color.b, 0);

        while(curtime < m_LifeTime)
        {
            curtime += Time.deltaTime;
            if (curtime > startfadetime)
            {
                m_Material.color = Color.Lerp(m_Material.color, target, 0.1f);
            }
            yield return null;
        }      
    }

}
