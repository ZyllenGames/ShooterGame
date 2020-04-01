using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    public event System.Action MyEvent;

    public void Invoke()
    {
        MyEvent();
    }

}
