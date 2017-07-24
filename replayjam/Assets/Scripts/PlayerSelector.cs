using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class PlayerSelector : MonoBehaviour {

    public enum PlayerSelectorState
    {
        Unjoined,
        NameEntry,
        Joined
    }

    public int playerNum = 1;

    public PlayerSelectorState state = PlayerSelectorState.Unjoined;
    
    public SoundEffectHandler confirmSound;
    public SoundEffectHandler cancelSound;

    public Text playerLabel;
    public Text playerStatus;

    public NameEntryController nameEntry;

    XboxController controller;

    PlayerInfo pInfo;


    //used for name entry
    bool canMoveCursor = true;
    float letterChangeThreshold = 1.0f;
    float letterChangeValue = 0.0f;
    public float nameEntrySensitivity = 10.0f;

    // Use this for initialization
    void Start()
    {
        controller = (XboxController)playerNum;
        Color playerColor = Globals.Instance.GameManager.GetPlayerColor(playerNum);
        playerLabel.color = playerColor;
        playerStatus.color = playerColor;

        nameEntry.TextColor = playerColor;
        nameEntry.gameObject.SetActive(false);

        pInfo = new PlayerInfo();
        pInfo.playerNum = playerNum;
        pInfo.name = nameEntry.GetName();
        pInfo.roundsWon = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //if (playerJoined && !Globals.Instance.GameManager.joinedPlayers.Contains(playerNum))
        //{
        //    Leave();
        //}
        if (state == PlayerSelectorState.NameEntry)
        {
            float horizontal = XCI.GetAxisRaw(XboxAxis.LeftStickX, controller);
            float vertical = XCI.GetAxisRaw(XboxAxis.LeftStickY, controller);

            bool usingStick = false;

            if (Mathf.Abs(horizontal) > 0.8f)
            {
                usingStick = true;
                if (canMoveCursor)
                {
                    nameEntry.MoveCursor(horizontal > 0);
                    canMoveCursor = false;
                }
            }
            else
            {
                canMoveCursor = true; //reset moving the cursor each time the user stops pushing a direction
            }

            if (Mathf.Abs(vertical) > 0.3)
            {
                usingStick = true;
                letterChangeValue += (vertical * Time.deltaTime * nameEntrySensitivity);

                if (vertical > 0 && letterChangeValue > letterChangeThreshold)
                {
                    //move up one letter
                    nameEntry.ChangeLetter(true);
                    letterChangeValue = 0.0f;
                }
                else if (vertical < 0 && letterChangeValue < (letterChangeThreshold * -1))
                {
                    //move down one letter
                    nameEntry.ChangeLetter(false);
                    letterChangeValue = 0.0f;
                }
            }
            else
            {
                //letterChangeValue = 0.0f;
            }

            if (!usingStick)
            {
                if (XCI.GetButtonDown(XboxButton.DPadRight, controller))
                {
                    nameEntry.MoveCursor(true);
                }
                else if (XCI.GetButtonDown(XboxButton.DPadLeft, controller))
                {
                    nameEntry.MoveCursor(false);
                }

                if (XCI.GetButtonDown(XboxButton.DPadUp, controller))
                {
                    nameEntry.ChangeLetter(true);
                }
                else if (XCI.GetButtonDown(XboxButton.DPadDown, controller))
                {
                    nameEntry.ChangeLetter(false);
                }
            }

            if (XCI.GetButtonDown(XboxButton.Y, controller))
            {
                nameEntry.SetRandomName();
            }
        }

        if (XCI.GetButtonDown(XboxButton.A, controller))
        {
            if (state != PlayerSelectorState.Joined)
            {
                if (confirmSound != null) { confirmSound.PlayEffect(); }
                Confirm();
            }
        }

        if (XCI.GetButtonDown(XboxButton.B, controller)) {
            if (state != PlayerSelectorState.Unjoined)
            {
                if (cancelSound != null) { cancelSound.PlayEffect(); }
                Cancel();
            }
        }

        if (XCI.GetButtonDown(XboxButton.Start, controller))
        {
            if (state == PlayerSelectorState.Joined && Globals.Instance.GameManager.isRoundReady)
            {
                //start game!
                Globals.Instance.GameManager.StartRound();
            }
        }
	}

    public void Confirm()
    {
        if (state == PlayerSelectorState.Unjoined)
        {
            state = PlayerSelectorState.NameEntry;
            nameEntry.gameObject.SetActive(true);
            nameEntry.DisplayTextEntry();

            if (playerStatus != null)
            {
                playerStatus.text = "A to confirm. B to cancel.";
            }

        } else if (state == PlayerSelectorState.NameEntry)
        {
            state = PlayerSelectorState.Joined;
            nameEntry.DisplayName();

            pInfo.name = nameEntry.GetName();

            Globals.Instance.GameManager.AddPlayer(pInfo);

            if (playerStatus != null)
            {
                playerStatus.text = "Joined! Press B to cancel...";
            }
        }
    }

    public void Cancel()
    {
        if (state == PlayerSelectorState.Joined)
        {
            state = PlayerSelectorState.NameEntry;
            nameEntry.gameObject.SetActive(true);
            nameEntry.DisplayTextEntry();

            if (playerStatus != null)
            {
                playerStatus.text = "A to confirm. B to cancel.";
            }

            Globals.Instance.GameManager.RemovePlayer(pInfo);

        }
        else if (state == PlayerSelectorState.NameEntry)
        {
            Reset();
        }
    }

    public void Reset()
    {
        playerStatus.text = "Press A to join!";
        state = PlayerSelectorState.Unjoined;

        nameEntry.gameObject.SetActive(false);

        Globals.Instance.GameManager.RemovePlayer(pInfo);

        pInfo.playerNum = playerNum;
        pInfo.name = nameEntry.GetName();
        pInfo.roundsWon = 0;
    }
}
