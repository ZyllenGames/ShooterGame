using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    IState m_CurState;
    IState m_PreState;
    public void ChangeState(IState newState)
    {
        if (m_CurState != null)
            m_CurState.ExitState();
        m_PreState = m_CurState;
        m_CurState = newState;
        m_CurState.EnterState();
    }

    public void StateUpdate(float sqrdist)
    {
        if (m_CurState != null)
            m_CurState.UpdateState(sqrdist);
    }

    public void ReturnToPreState()
    {
        if (m_CurState != null)
            m_CurState.ExitState();
        IState temp = m_CurState;
        m_CurState = m_PreState;
        m_PreState = temp;
        if (m_CurState != null)
            m_CurState.EnterState();
    }



}
