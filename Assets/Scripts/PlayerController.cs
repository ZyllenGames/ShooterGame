using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody m_Rigidbody;

    public Vector3 a_Velocity { get; set; }
    public Vector3 a_LookAtPoint { get; set; }


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    private void Update()
    {
        //rotate
        transform.LookAt(a_LookAtPoint);
    }

    private void FixedUpdate()
    {
        //move
        m_Rigidbody.MovePosition(m_Rigidbody.position + a_Velocity * Time.fixedDeltaTime);
    }
}
