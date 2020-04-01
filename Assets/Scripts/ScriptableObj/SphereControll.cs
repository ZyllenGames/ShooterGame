using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereControll : MonoBehaviour
{
    public GameEvent Event;

    private void Awake()
    {
        Event.MyEvent += OnPlayerHealthDown;
    }

    void OnPlayerHealthDown()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Event.MyEvent -= OnPlayerHealthDown;
    }
}
