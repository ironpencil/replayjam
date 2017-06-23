using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class PlayerSelector : MonoBehaviour {

    public int playerNum = 1;
    public bool playerJoined = false;
    public Image AButton;
    public Image BButton;

    public SoundEffectHandler confirmSound;
    public SoundEffectHandler cancelSound;
    
    public Text playerStatus;

    public GameObject colorSelector;

    XboxController controller;


    // Use this for initialization
    void Start () {
        BButton.enabled = false;
        controller = (XboxController)playerNum;
        colorSelector.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (playerJoined && !Globals.Instance.GameManager.joinedPlayers.Contains(playerNum))
        {
            Leave();
        }

        if (XCI.GetButtonDown(XboxButton.A, controller))
        {
            if (!playerJoined)
            {
                confirmSound.PlayEffect();
                Join();
            }
        }

        if (XCI.GetButtonDown(XboxButton.B, controller)) {
            if (playerJoined)
            {
                cancelSound.PlayEffect();
                Leave();
            }
        }

        if (XCI.GetButtonDown(XboxButton.Start, controller))
        {
            if (playerJoined)
            {
                //start game!
                Globals.Instance.GameManager.StartRound();
            }
        }

        if (XCI.GetButtonDown(XboxButton.Back, controller))
        {
            Globals.Instance.GameManager.DisplayOptions();
        }
	}

    public void Join()
    {
        playerJoined = true;
        Globals.Instance.GameManager.AddPlayer(playerNum);
        if (playerStatus != null)
        {
            AButton.enabled = false;
            BButton.enabled = true;
            playerStatus.text = "Joined! Press       to cancel...";
        }
        colorSelector.SetActive(true);
    }

    public void Leave()
    {
        AButton.enabled = true;
        BButton.enabled = false;
        playerStatus.text = "Press       to join!";
        playerJoined = false;
        Globals.Instance.GameManager.RemovePlayer(playerNum);
        colorSelector.SetActive(false);
    }
}
