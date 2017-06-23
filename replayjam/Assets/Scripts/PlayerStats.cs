using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {
    public int playerNum;
    public int wins;
    public float survivalTime;
    public float bombTargets;
    public List<Kill> kills;
    public List<Kill> deaths;

    public PlayerStats()
    {
        kills = new List<Kill>();
        deaths = new List<Kill>();
    }

    public int GetOtherKillCount()
    {
        int killOtherCount = 0;

        foreach (Kill kill in kills)
        {
            if (kill.victimPlayerNum != kill.killerPlayerNum) killOtherCount++;
        }

        return killOtherCount;
    }
}
