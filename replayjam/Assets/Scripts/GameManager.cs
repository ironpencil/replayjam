using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int numPlayers = 2;

    public List<int> joinedPlayers;

    public List<PlayerInput> livingPlayers;

    public bool isRoundActive = false;
    public bool isRoundReady = false;

    public GameObject playerPrefab;
    public GameObject ringPrefab;
    public Color player1Color = Color.red;
    public Color player2Color = Color.green;
    public Color player3Color = Color.blue;
    public Color player4Color = Color.yellow;

    // Use this for initialization
    void Start () {
        
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

    public void SetupGame()
    {
        CleanupRound();

        
        StartRound();
        
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

        livingPlayers.Clear();

        isRoundActive = true;
        isRoundReady = false;

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
}
