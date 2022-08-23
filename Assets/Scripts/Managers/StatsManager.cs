using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/StatsManager")]
[Serializable]
public class StatsManager : ScriptableObject
{
    // New stats
    public int s_UserDeaths;
    public int s_StarsEarned;

    public void AddDeath()
    {
        s_UserDeaths += 1;
    }
    
    
}
