using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Singleton/MasterManager")]
public class MaterManager : SingletonSO<MaterManager>
{
    public int heatlth;

    public void PrintHealth()
    {
        Debug.Log(heatlth);
    }
}
