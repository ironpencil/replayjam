using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int numPlayers = 2;

    public List<int> joinedPlayers = new List<int>();

    public List<PlayerInput> livingPlayers;

    public bool isRoundActive = false;
    public bool isRoundReady = false;

    public GameObject playerPrefab;
    public GameObject ringPrefab;
    public Color player1Color = Color.red;
    public Color player2Color = Color.green;
    public Color player3Color = Color.blue;
    public Color player4Color = Color.yellow;

    public PlayerSelectController playerSelect;

    // Use this for initialization
    void Start () {
        
    }

    

    public void SetupGame()
    {
        CleanupRound();

        playerSelect.gameObject.SetActive(true);   
    }
    
    void UpdateHud()
    {
        
    }

    // Update is called once per frame
    void Update () {

        if (isRoundActive)
        {
            
        } else
        {
            if (isRoundReady && Input.anyKeyDown)
            {
                StartRound();
            }
        }
	}

    public void AddPlayer(int playerNum)
    {
        if (!joinedPlayers.Contains(playerNum)) { joinedPlayers.Add(playerNum); }
    }

    public void RemovePlayer(int playerNum)
    {
       if (joinedPlayers.Contains(playerNum)) { joinedPlayers.Remove(playerNum); }
    }

    public void DisplayPlayerSetup()
    {
        
    }

    public void StartRound()
    {

        
        isRoundActive = true;
        isRoundReady = false;

        playerSelect.gameObject.SetActive(false);

        joinedPlayers = joinedPlayers.Where(p => p > 0).ToList();
        joinedPlayers.Sort();

        numPlayers = joinedPlayers.Count;

        for (int i = 0; i < numPlayers; i++)
        {
            SpawnPlayer(joinedPlayers[i], i + 1);
        }

        
    }

   

    IEnumerator EndRound()
    {
        isRoundActive = false;
        Time.timeScale = 0.0f;

        

        yield return new WaitForSecondsRealtime(2.0f);

        CleanupRound();

        Time.timeScale = 1.0f;

    }

    private void CleanupRound()
    {


        foreach (Transform trans in Globals.Instance.dynamicsParent)
        {
            Destroy(trans.gameObject);
        }

        livingPlayers.Clear();

    }



    internal Color GetPlayerColor(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                return player1Color;
            case 2:
                return player2Color;
            case 3:
                return player3Color;
            case 4:
                return player4Color;
        }

        return Color.white;
    }

    internal void SetPlayerColor(Color color, int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                player1Color = color;
                break;
            case 2:
                player2Color = color;
                break;
            case 3:
                player3Color = color;
                break;
            case 4:
                player4Color = color;
                break;
        }
    }


    void SpawnPlayer(int playerNum, int playerPosition)
    {
        GameObject player = GameObject.Instantiate(playerPrefab, Globals.Instance.dynamicsParent);
        PlayerInput playerScript = player.GetComponent<PlayerInput>();
        playerScript.playerNum = playerNum;

        playerScript.playerPosition = playerPosition;

        GameObject playerRing = GameObject.Instantiate(ringPrefab, Globals.Instance.dynamicsParent);
        playerScript.playerRing = playerRing;

        livingPlayers.Add(playerScript);
    }


    public void KillPlayer(int playerNum)
    {
        PlayerInput pi = livingPlayers.FirstOrDefault(p => p.playerNum == playerNum);

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
}
