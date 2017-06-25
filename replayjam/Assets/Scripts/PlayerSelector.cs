using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class PlayerSelector : MonoBehaviour {

    public int playerNum = 1;
    public bool playerJoined = false;

    public SoundEffectHandler confirmSound;
    public SoundEffectHandler cancelSound;

    public Text playerName;
    public Text playerStatus;

    XboxController controller;


    // Use this for initialization
    void Start () {
        controller = (XboxController)playerNum;
        Color playerColor = Globals.Instance.GameManager.GetPlayerColor(playerNum);
        playerName.color = playerColor;
        playerStatus.color = playerColor;
    }
	
	// Update is called once per frame
	void Update () {
        //if (playerJoined && !Globals.Instance.GameManager.joinedPlayers.Contains(playerNum))
        //{
        //    Leave();
        //}

        if (XCI.GetButtonDown(XboxButton.A, controller))
        {
            if (!playerJoined)
            {
                if (confirmSound != null) { confirmSound.PlayEffect(); }
                Join();
            }
        }

        if (XCI.GetButtonDown(XboxButton.B, controller)) {
            if (playerJoined)
            {
                if (cancelSound != null) { cancelSound.PlayEffect(); }
                Leave();
            }
        }

        if (XCI.GetButtonDown(XboxButton.Start, controller))
        {

            //todo: make it so that game can only start with 2 or more players
            if (playerJoined)
            {
                //start game!
                Globals.Instance.GameManager.StartRound();
            }
        }
	}

    public void Join()
    {
        playerJoined = true;
        PlayerInfo pi = new PlayerInfo();
        pi.playerNum = playerNum;
        pi.name = "Player " + playerNum;
        pi.roundsWon = 0;
        Globals.Instance.GameManager.AddPlayer(pi);
        if (playerStatus != null)
        {
            playerStatus.text = "Joined! Press B to cancel...";
        }
    }

    public void Leave()
    {
        playerStatus.text = "Press A to join!";
        playerJoined = false;
        Globals.Instance.GameManager.RemovePlayer(playerNum);
    }
}
