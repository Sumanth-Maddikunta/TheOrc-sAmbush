using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public delegate void OnEnemyDeath(uint score,int ammo);
    public  static event OnEnemyDeath OnEnemyDeathEvent;
    
    public static void RaiseEvent(uint score,int ammo)
    {
        if(OnEnemyDeathEvent!=null)
        {
            OnEnemyDeathEvent(score,ammo);
        }
    }
}
