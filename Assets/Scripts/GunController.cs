using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public Transform HoldPosition;
    public Gun StartGun;

    Gun m_EquiptedGun;

    Transform m_Target;

    private void Awake()
    {
        if(StartGun != null)
        {
            EquiptGun(StartGun);
        }
    }

    private void Update()
    {
        if(m_EquiptedGun != null)
        {
            Transform gungraphics = m_EquiptedGun.transform.GetChild(0);
            gungraphics.position = Vector3.Lerp(gungraphics.position, HoldPosition.position, 0.2f);
            gungraphics.rotation = Quaternion.Lerp(gungraphics.rotation, HoldPosition.rotation, 0.2f);
        }
        else
        {
            EquiptGun(StartGun);
        }
        
    }

    public void EquiptGun(Gun guntoequipt)
    {
        if(m_EquiptedGun != null)
        {
            Destroy(m_EquiptedGun.gameObject);
        }
        m_EquiptedGun = Instantiate(guntoequipt, HoldPosition.position, HoldPosition.rotation);
        m_EquiptedGun.transform.parent = HoldPosition;
    }

    public void Shot()
    {
        if(m_EquiptedGun != null)
        {
            m_EquiptedGun.ShotProjectiles();
        }
    }

    public void SetTarget(Transform trans)
    {
        m_Target = trans;
    }

    public void AIShot()
    {
        if (m_EquiptedGun != null)
        {
            m_EquiptedGun.SetShootAt(m_Target);
            m_EquiptedGun.ShotProjectiles();
        }
    }
}
