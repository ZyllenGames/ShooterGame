using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    Rigidbody m_Player;
    public Vector3 Offset = new Vector3(0, 6, -2);
    public float Speed = 5;
    private void Awake()
    {
        m_Player = FindObjectOfType<Player>().GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if(m_Player != null)
        {
            Vector3 target = m_Player.position + Offset;
            transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.fixedDeltaTime);
        }
        
    }
}
