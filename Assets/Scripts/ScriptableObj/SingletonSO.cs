using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SingletonSO<T> : ScriptableObject where T : ScriptableObject
{
    private static T m_Instance = null;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault<T>();
            return m_Instance;
        }
    }
}
