using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipLibrary : MonoBehaviour
{
    public ClipGroup[] ClipLib;

    Dictionary<string, AudioClip[]> m_ClipDictionary;

    private void Awake()
    {
        m_ClipDictionary = new Dictionary<string, AudioClip[]>();
        for (int i = 0; i < ClipLib.Length; i++)
        {
            m_ClipDictionary.Add(ClipLib[i].name, ClipLib[i].clipgroup);
        }
    }

    public AudioClip GetClipByName(string name)
    {
        if(m_ClipDictionary.ContainsKey(name))
        {
            AudioClip[] audioclip = m_ClipDictionary[name];
            return audioclip[Random.Range(0, audioclip.Length)];

        }
        return null;
    }

    [System.Serializable]
    public class ClipGroup
    {
        public string name;
        public AudioClip[] clipgroup;
    }
}
