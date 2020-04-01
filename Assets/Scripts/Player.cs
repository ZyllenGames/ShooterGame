using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    public float Speed = 10f;
    PlayerController m_PlayerController;
    Camera m_Camera;

    GunController m_GunController;

    public event System.Action PlayerDead;
    public event System.Action<float> PlayerLifeChange;

    protected override void Awake()
    {
        base.Awake();
        m_PlayerController = GetComponent<PlayerController>();
        m_GunController = GetComponent<GunController>();
        m_Camera = Camera.main;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movement Input
        Vector3 inputMove = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 dirMove = inputMove.normalized;
        if (inputMove.magnitude > 1)
            inputMove.Normalize();
        Vector3 velocity = inputMove * Speed;
        m_PlayerController.a_Velocity = velocity;

        //LootAt Input
        Ray mouseRay = m_Camera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float dist2Ground;
        
        if(ground.Raycast(mouseRay, out dist2Ground))
        {
            Vector3 hitPoint = mouseRay.GetPoint(dist2Ground);
            //Debug.DrawLine(mouseRay.origin, hitPoint, Color.red);
            Vector3 correctLookPostion = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
            m_PlayerController.a_LookAtPoint = correctLookPostion;
        }

        //Shot Gun input
        if(Input.GetMouseButton(0))
        {
            m_GunController.Shot();
        }
    }

    public override void Die(Vector3 hitPos, Vector3 hitDir)
    {
        base.Die(hitPos, hitDir);
        if(PlayerDead != null)
            PlayerDead();
    }

    public override void onHit(float damage, Vector3 hitPos, Vector3 hitDir)
    {
        base.onHit(damage, hitPos, hitDir);
        float liferatio = 0;
        if (m_Health < 0)
            m_Health = 0;
        liferatio = m_Health / StartHealth;

        if(PlayerLifeChange != null)
            PlayerLifeChange(liferatio);
    }

    public void onHeal(float heal)
    {
        m_Health += heal;
        if (m_Health >= StartHealth)
            m_Health = StartHealth;

        float liferatio = 0;
        liferatio = m_Health / StartHealth;
        if (PlayerLifeChange != null)
            PlayerLifeChange(liferatio);
    }
}
