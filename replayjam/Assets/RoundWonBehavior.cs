using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class RoundWonBehavior : MonoBehaviour {

    private GameManager gm;

    public List<Text> playerNames;
    public List<Text> playerScores;

    public Text winnerText;

	// Use this for initialization
	void Start () {
        gm = Globals.Instance.GameManager;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateFields();

        for (int i = 1; i <= 4; i++)
        {
            if (XCI.GetButtonDown(XboxButton.A, (XboxController)i))
            {
                if (gm.lastRoundWinner.roundsWon > 2)
                {
                    gm.EndGame();
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(false);
                    gm.StartRound();
                }
            }
        }
    }

    public void DisplayScreen()
    {
        UpdateFields();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateFields()
    {
        if (gm == null) gm = Globals.Instance.GameManager;
        winnerText.text = gm.lastRoundWinner.name + " Wins the Round!";
        winnerText.color = gm.GetPlayerColor(gm.lastRoundWinner.playerNum);

        foreach (Text t in playerScores)
        {
            t.transform.parent.gameObject.SetActive(false);
        }

        int i = 0;

        foreach (PlayerInfo pi in gm.joinedPlayers)
        {
            playerNames[i].transform.parent.gameObject.SetActive(true);

            Color playerColor = gm.GetPlayerColor(pi.playerNum);
            playerNames[i].color = playerColor;
            playerNames[i].text = pi.name;

            playerScores[i].color = playerColor;
            playerScores[i].text =  pi.roundsWon + "";

            i++;
        }
    }
}
