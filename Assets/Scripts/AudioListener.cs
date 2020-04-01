using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    Transform m_Player;

    private void Awake()
    {
        m_Player = FindObjectOfType<Player>().transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Player != null)
            transform.position = m_Player.position;
    }
}
