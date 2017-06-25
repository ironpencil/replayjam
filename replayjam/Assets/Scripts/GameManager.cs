using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class GameManager : MonoBehaviour {

    public int numPlayers = 2;

    public PlayerInfo lastRoundWinner;

    public List<PlayerInfo> joinedPlayers = new List<PlayerInfo>();
    public List<PlayerInput> livingPlayers;

    public bool isRoundActive = false;
    public bool isRoundReady = false;

    public GameObject playerPrefab;
    public GameObject ringPrefab;
    public List<Color> playerColors = new List<Color>() { Color.red, Color.green, Color.blue, Color.yellow };
    public List<Sprite> playerSprites;

    public PlayerSelectController playerSelect;
    public RoundWonBehavior roundWon;
    public VictoryBehavior victory;

    // Use this for initialization
    void Start () {
        
    }
    
    public void EndGame()
    {
        CleanupRound();
        victory.gameObject.SetActive(true);
    }

    public void SetupGame()
    {
        CleanupRound();
        playerSelect.gameObject.SetActive(true);
    }
    
    // Update is called once per frame
    void Update () {
        
        if (isRoundActive)
        {
            if (CheckForExitInput((XboxController)1))
            {
                SetupGame();
            }
            else {
                //still playing
                if (livingPlayers.Count == 1)
                {
                    lastRoundWinner = livingPlayers[0].playerInfo;
                    lastRoundWinner.roundsWon++;
                    StartCoroutine(EndRound());
                }
                else if (livingPlayers.Count == 0)
                {
                    lastRoundWinner = null;
                    StartCoroutine(EndRound());
                }
            }
        } else
        {
            if (CheckForExitInput((XboxController)1)) {
                Globals.Instance.DoQuit();
            }

            //if (isRoundReady && Input.anyKeyDown)
            //{
            //    StartRound();
            //}
        }
	}

    public void AddPlayer(PlayerInfo player)
    {
        if (!joinedPlayers.Contains(player)) { joinedPlayers.Add(player); }

        CheckPlayerCount();
    }

    public void RemovePlayer(PlayerInfo player)
    {
        joinedPlayers.Remove(player);

        CheckPlayerCount();
    }

    void CheckPlayerCount()
    {
        bool wasRoundReady = isRoundReady;
        isRoundReady = joinedPlayers.Count > 1;

        if (wasRoundReady != isRoundReady)
        {
            playerSelect.DisplayStartGame(isRoundReady);
        }
    }

    public void DisplayPlayerSetup()
    {
        
    }

    public void StartRound()
    {
        isRoundActive = true;
        isRoundReady = false;

        playerSelect.gameObject.SetActive(false);
        joinedPlayers = joinedPlayers.Where(p => p.playerNum > 0).OrderBy(p => p.playerNum).ToList();

        numPlayers = joinedPlayers.Count;

        for (int i = 0; i < numPlayers; i++)
        {
            SpawnPlayer(joinedPlayers[i], i + 1);
        }

        if (numPlayers == 4)
        {
            //swap 3 and 4 positions to read left->right instead of clockwise around a circle
            livingPlayers[2].playerPosition = 4;
            livingPlayers[3].playerPosition = 3;
        }

    }

    IEnumerator EndRound()
    {
        isRoundActive = false;
        //Time.timeScale = 0.0f;
        
        yield return new WaitForSecondsRealtime(2.0f);
        
        CleanupRound();
        //Time.timeScale = 1.0f;
        roundWon.DisplayScreen();
    }

    private void CleanupRound()
    {
        isRoundActive = false;

        foreach (Transform trans in Globals.Instance.dynamicsParent)
        {
            Destroy(trans.gameObject);
        }

        livingPlayers.Clear();

        joinedPlayers = new List<PlayerInfo>();

        playerSelect.ResetScreen();

    }



    internal Color GetPlayerColor(int playerNum)
    {
        return playerColors[playerNum - 1];
    }

    internal void SetPlayerColor(Color color, int playerNum)
    {
        playerColors[playerNum - 1] = color;
    }


    void SpawnPlayer(PlayerInfo pi, int playerPosition)
    {
        GameObject player = GameObject.Instantiate(playerPrefab, Globals.Instance.dynamicsParent);
        PlayerInput playerScript = player.GetComponent<PlayerInput>();

        playerScript.playerPosition = playerPosition;

        GameObject playerRing = GameObject.Instantiate(ringPrefab, Globals.Instance.dynamicsParent);
        playerScript.playerRing = playerRing;
        playerScript.playerInfo = pi;
        playerScript.playerShip.sprite = playerSprites[pi.playerNum - 1];

        livingPlayers.Add(playerScript);
    }


    public void KillPlayer(int playerNum)
    {
        PlayerInput pi = livingPlayers.FirstOrDefault(p => p.playerInfo.playerNum == playerNum);

        if (pi != null)
        {
            PlayerInput pos1 = livingPlayers.FirstOrDefault(p => p.playerPosition == 1);
            PlayerInput pos2 = livingPlayers.FirstOrDefault(p => p.playerPosition == 2);
            PlayerInput pos3 = livingPlayers.FirstOrDefault(p => p.playerPosition == 3);
            PlayerInput pos4 = livingPlayers.FirstOrDefault(p => p.playerPosition == 4);

            livingPlayers.Remove(pi);

            //get player position
            int removedPosition = pi.playerPosition;

            int remainingPlayers = livingPlayers.Count;

            if (remainingPlayers == 3)
            {
                switch (removedPosition)
                {
                    case 1:
                        pos2.AdjustHingeLimits(-45, -15); //ccw
                        pos4.AdjustHingeLimits(15, 45); //cw
                        pos3.AdjustHingeLimits(-15, 15); //both
                        pos2.playerPosition--;
                        pos3.playerPosition--;
                        pos4.playerPosition--;
                        break;
                    case 2:
                        pos3.AdjustHingeLimits(-45, -15); //ccw
                        pos1.AdjustHingeLimits(15, 45); //cw
                        pos4.AdjustHingeLimits(-15, 15); //both
                        pos3.playerPosition--;
                        pos4.playerPosition--;
                        break;
                    case 3:
                        pos4.AdjustHingeLimits(-45, -15); //ccw
                        pos2.AdjustHingeLimits(15, 45); //cw
                        pos1.AdjustHingeLimits(-15, 15); //both
                        pos4.playerPosition--;
                        break;
                    case 4:
                        pos1.AdjustHingeLimits(-45, -15); //ccw
                        pos3.AdjustHingeLimits(15, 45); //cw
                        pos2.AdjustHingeLimits(-15, 15); //both
                        break;
                    default:
                        break;
                }
            }
            else if (remainingPlayers == 2)
            {
                switch (removedPosition)
                {
                    case 1:
                        pos2.AdjustHingeLimits(-60, 0); //ccw
                        pos3.AdjustHingeLimits(0, 60); //cw
                        pos2.playerPosition--;
                        pos3.playerPosition--;
                        break;
                    case 2:
                        pos3.AdjustHingeLimits(-60, 0); //ccw
                        pos1.AdjustHingeLimits(0, 60); //cw
                        pos3.playerPosition--;
                        break;
                    case 3:
                        pos1.AdjustHingeLimits(-60, 0); //ccw
                        pos2.AdjustHingeLimits(0, 60); //cw
                        break;
                    default:
                        break;
                }
            }
            else if (remainingPlayers == 1)
            {
                //someone won!
            }
            else
            {
                // wtf?? all dead??
            }
        }
    }

    bool CheckForExitInput(XboxController controller)
    {
        bool doExit = 
            (XCI.GetButton(XboxButton.Start, controller) && XCI.GetButtonDown(XboxButton.Back, controller)) ||
            (XCI.GetButton(XboxButton.Back, controller) && XCI.GetButtonDown(XboxButton.Start, controller)) ||
            Input.GetKeyDown(KeyCode.Escape);

        return doExit;
    }
}
