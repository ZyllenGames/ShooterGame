using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    float m_HealthRec = 5;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().onHeal(m_HealthRec);
            Destroy(gameObject);
        }
    }
}
