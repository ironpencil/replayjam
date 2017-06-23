using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class ScoreScreenBehavior : MonoBehaviour {
    public Text player1Text;
    public Text player2Text;
    public Text player3Text;
    public Text player4Text;
    public Text playerWinsText;
    public VictoryScreenBehavior victoryScreenUI;
    GameManager gm;

    public SoundEffectHandler endGameSound;

    public void Display()
    {
        gm = Globals.Instance.GameManager;

        player1Text.gameObject.SetActive(false);
        player2Text.gameObject.SetActive(false);
        player3Text.gameObject.SetActive(false);
        player4Text.gameObject.SetActive(false);

        //Debug.Log("Player Stats: " + gm.playerStats);

        foreach (PlayerStats ps in gm.playerStats.Values)
        {
            switch (ps.playerNum)
            {
                case 1:
                    player1Text.text = GetScoreText(ps);
                    player1Text.gameObject.SetActive(true);
                    break;
                case 2:
                    player2Text.text = GetScoreText(ps);
                    player2Text.gameObject.SetActive(true);
                    break;
                case 3:
                    player3Text.text = GetScoreText(ps);
                    player3Text.gameObject.SetActive(true);
                    break;
                case 4:
                    player4Text.text = GetScoreText(ps);
                    player4Text.gameObject.SetActive(true);
                    break;
            }
        }

        if (gm.lastRoundWinner == 0)
        {
            playerWinsText.text = "Draw!";
        }
        else
        {
            playerWinsText.text = "Player " + gm.lastRoundWinner + " Wins the Round!";
        }
        
        gameObject.SetActive(true);
    }

    string GetScoreText(PlayerStats ps)
    {
        return "Wins: " + ps.wins +
                "\nTime Alive: " + Math.Round(ps.survivalTime, 1) + " seconds" +
                "\nKills: " + ps.GetOtherKillCount();
    }

    // Use this for initialization
    void Start () {
  
	}

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (XCI.GetButtonDown(XboxButton.A, (XboxController)i))
            {
                if (gm.GetWinner() != null)
                {
                    gameObject.SetActive(false);
                    victoryScreenUI.Display();
                    endGameSound.PlayEffect();
                }
                else
                {
                    gm.StartRound();
                }
            }
        }
    }
}
