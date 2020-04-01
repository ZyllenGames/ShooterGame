using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    int m_Points;


    public event System.Action<int> PointsChange;

    private void Awake()
    {
        m_Points = 0;
        AIEnemy.OnKilledStatic += OnEnemyKilled;
    }

    void OnEnemyKilled()
    {
        m_Points += 5;
        PointsChange(m_Points);
    }

    private void OnDestroy()
    {
        AIEnemy.OnKilledStatic -= OnEnemyKilled;
    }
}
