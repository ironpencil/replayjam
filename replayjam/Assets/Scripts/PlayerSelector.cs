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
    public SoundEffectHandler joinedConfirmSound;
    public SoundEffectHandler joinedCancelSound;

    public SoundEffectHandler moveCursorSound;
    public SoundEffectHandler changeLetterNextSound;
    public SoundEffectHandler changeLetterPrevSound;
    public SoundEffectHandler randomNameSound;

    public Text playerLabel;
    public Text playerStatus;

    public NameEntryController nameEntry;
    public GameObject readyText;

    XboxController controller;

    PlayerInfo pInfo = new PlayerInfo();


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
                    if (moveCursorSound != null) { moveCursorSound.PlayEffect(); }
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
                    if (changeLetterNextSound != null) { changeLetterNextSound.PlayEffect(); }
                }
                else if (vertical < 0 && letterChangeValue < (letterChangeThreshold * -1))
                {
                    //move down one letter
                    nameEntry.ChangeLetter(false);
                    letterChangeValue = 0.0f;
                    if (changeLetterPrevSound != null) { changeLetterPrevSound.PlayEffect(); }
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
                    if (moveCursorSound != null) { moveCursorSound.PlayEffect(); }
                }
                else if (XCI.GetButtonDown(XboxButton.DPadLeft, controller))
                {
                    nameEntry.MoveCursor(false);
                    if (moveCursorSound != null) { moveCursorSound.PlayEffect(); }
                }

                if (XCI.GetButtonDown(XboxButton.DPadUp, controller))
                {
                    nameEntry.ChangeLetter(true);
                    if (changeLetterNextSound != null) { changeLetterNextSound.PlayEffect(); }
                }
                else if (XCI.GetButtonDown(XboxButton.DPadDown, controller))
                {
                    nameEntry.ChangeLetter(false);
                    if (changeLetterPrevSound != null) { changeLetterPrevSound.PlayEffect(); }
                }
            }

            if (XCI.GetButtonDown(XboxButton.Y, controller))
            {
                nameEntry.SetRandomName();
                if (randomNameSound != null) { randomNameSound.PlayEffect(); }
            }
        }

        if (XCI.GetButtonDown(XboxButton.A, controller))
        {
            Confirm();
        }

        if (XCI.GetButtonDown(XboxButton.B, controller))
        {
            Cancel();
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
            if (confirmSound != null) { confirmSound.PlayEffect(); }
            state = PlayerSelectorState.NameEntry;
            nameEntry.gameObject.SetActive(true);
            nameEntry.DisplayTextEntry();

            if (playerStatus != null)
            {
                playerStatus.text = "A: Confirm     B: Cancel\r\nY: Random Name";
            }

        } else if (state == PlayerSelectorState.NameEntry)
        {
            if (joinedConfirmSound != null) { joinedConfirmSound.PlayEffect(); }
            state = PlayerSelectorState.Joined;
            nameEntry.DisplayName();

            readyText.SetActive(true);

            pInfo.name = nameEntry.GetName();

            Globals.Instance.GameManager.AddPlayer(pInfo);

            if (playerStatus != null)
            {
                playerStatus.text = "Joined!\r\nPress B to cancel...";
            }
        }
    }

    public void Cancel()
    {
        if (state == PlayerSelectorState.Joined)
        {
            if (joinedCancelSound != null) { joinedCancelSound.PlayEffect(); }
            state = PlayerSelectorState.NameEntry;
            nameEntry.gameObject.SetActive(true);
            nameEntry.DisplayTextEntry();

            readyText.SetActive(false);

            if (playerStatus != null)
            {
                playerStatus.text = "A: Confirm     B: Cancel\r\nY: Random Name";
            }

            Globals.Instance.GameManager.RemovePlayer(pInfo);

        }
        else if (state == PlayerSelectorState.NameEntry)
        {
            if (cancelSound != null) { cancelSound.PlayEffect(); }
            Reset();
        }
    }

    public void Reset()
    {
        playerStatus.text = "Press A to join!";
        state = PlayerSelectorState.Unjoined;

        nameEntry.gameObject.SetActive(false);
        readyText.SetActive(false);

        Globals.Instance.GameManager.RemovePlayer(pInfo);

        pInfo.playerNum = playerNum;
        pInfo.name = nameEntry.GetName();
        pInfo.roundsWon = 0;
    }
}
