using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class SObjTest : ScriptableObject
{
    private void OnEnable()
    {
        Debug.Log("Enable Called");
    }

    private void OnDisable()
    {
        Debug.Log("Disable Called");
    }

    [SerializeField]
    string MyName;
    [SerializeField]
    public int Heath;
    [SerializeField]
    int m_Number;
}
