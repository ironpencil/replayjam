using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundWonBehavior : MonoBehaviour {

    private GameManager gm;
    public List<Text> playerScores;
    public Text winnerText;

	// Use this for initialization
	void Start () {
        gm = Globals.Instance.GameManager;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateFields();
    }

    public void Display()
    {
        UpdateFields();
        enabled = true;
    }

    public void Hide()
    {
        enabled = false;
    }

    public void UpdateFields()
    {
        winnerText.text = gm.lastRoundWinner.name + " Wins the Round!";

        foreach (Text t in playerScores)
        {
            t.text = "";
        }

        int i = 0;

        foreach (PlayerInfo pi in gm.joinedPlayers)
        {
            playerScores[i++].text = pi.name + "\n\n" + pi.roundsWon;
        }
    }
}
