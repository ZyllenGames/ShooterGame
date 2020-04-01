using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState
{
    protected GameObject OwnerObject;

    public IState(GameObject owner)
    {
        OwnerObject = owner;
    }

    public abstract void EnterState();

    public abstract void UpdateState(float sqrdist);

    public abstract void ExitState();
}
