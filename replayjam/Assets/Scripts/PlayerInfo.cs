using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {
    public string name;
    public int roundsWon;
    public int playerNum;

    public bool Equals(PlayerInfo other)
    {
        if (other == this) return true;
        if (other.name.Equals(this.name)) return true;
        return false;
    }
}
