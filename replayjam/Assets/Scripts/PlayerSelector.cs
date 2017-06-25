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

    PlayerInfo pInfo;
    
    // Use this for initialization
    void Start () {
        controller = (XboxController)playerNum;
        Color playerColor = Globals.Instance.GameManager.GetPlayerColor(playerNum);
        playerName.color = playerColor;
        playerStatus.color = playerColor;

        pInfo = new PlayerInfo();
        pInfo.playerNum = playerNum;
        pInfo.name = "Player " + playerNum;
        pInfo.roundsWon = 0;
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

            if (playerJoined && Globals.Instance.GameManager.isRoundReady)
            {
                //start game!
                Globals.Instance.GameManager.StartRound();
            }
        }
	}

    public void Join()
    {
        playerJoined = true;

        Globals.Instance.GameManager.AddPlayer(pInfo);
        if (playerStatus != null)
        {
            playerStatus.text = "Joined! Press B to cancel...";
        }
    }

    public void Leave()
    {
        playerStatus.text = "Press A to join!";
        playerJoined = false;

        Globals.Instance.GameManager.RemovePlayer(pInfo);

        pInfo = new PlayerInfo();
        pInfo.playerNum = playerNum;
        pInfo.name = "Player " + playerNum;
        pInfo.roundsWon = 0;
    }
}
