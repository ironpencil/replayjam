using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int numPlayers = 2;
    
    public bool isRoundActive = false;
    public bool isRoundReady = false;

    public GameObject playerPrefab;
    public Color player1Color = Color.red;
    public Color player2Color = Color.green;
    public Color player3Color = Color.blue;
    public Color player4Color = Color.yellow;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < numPlayers; i++)
        {
            SpawnPlayer(i + 1);
        }
    }

    void SpawnPlayer(int playerNum)
    {
        GameObject player = GameObject.Instantiate(playerPrefab, Globals.Instance.dynamicsParent);
        PlayerInput playerScript = player.GetComponent<PlayerInput>();
        playerScript.playerNum = playerNum;
    }

    public void SetupGame()
    {
        CleanupRound();

        
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
        
        
    }

    public void RemovePlayer(int playerNum)
    {
       
    }

    public void DisplayPlayerSetup()
    {
        
    }

    public void StartRound()
    {
        
        

        isRoundActive = true;
        isRoundReady = false;
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
