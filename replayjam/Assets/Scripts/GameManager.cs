using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    float deltaTime = 0.0f;
    public GameOptions gameOptions;

    public Dictionary<int, PlayerController> livingPlayers = new Dictionary<int, PlayerController>();
    public List<GameObject> spinners;
    public List<GameObject> lasers;
    public List<GameObject> wallBlades;
    public List<GameObject> mines;
    public List<GameObject> laserLines;
    public List<Kill> kills;
    public List<Text> hudScores;
    public Text hudRound;

    public Text hudTime;
    public Text hudFps;

    public List<int> joinedPlayers;

    private int numPlayers = 0;

    public int currentRound = 0;
    public Dictionary<int, PlayerStats> playerStats;
    public int lastRoundWinner = 0;
    public float roundStartTime = 0.0f;

    public List<Transform> playerSpawnPoints;

    public Transform dynamicsParent;
    public GameObject playerPrefab;
    public List<GameObject> weirdSpinnerPrefabs;
    public GameObject spinnerPrefab;
    public GameObject laserPrefab;
    public GameObject laserLinePrefab;
    public GameObject bombSpawnerPrefab;
    public GameObject wallBladePrefab;
    public GameObject mineSpawnerPrefab;
    public GameObject crateSpawnerPrefab;

    public GameObject playerSetupUI;
    public ScoreScreenBehavior scoreScreenUI;
    public GameObject optionsUI;
    
    public TileManager tileManager;

    public SoundEffectHandler startRoundSound;
    public SoundEffectHandler endRoundSound;
    
    public Vector2 minWallLaserSpawn;
    public Vector2 maxWallLaserSpawn;
    
    public bool isRoundActive = false;
    public bool isRoundReady = false;

    public Color player1Color;
    public Color player2Color;
    public Color player3Color;
    public Color player4Color;

    // Use this for initialization
    void Start () {
    }

    public void SetupGame()
    {
        CleanupRound();

        int i = 1;
        foreach (Text text in hudScores)
        {
            text.text = "Player " + i++ + ": " + 0;
            text.gameObject.SetActive(false);
        }
        hudRound.gameObject.SetActive(false);
        hudTime.gameObject.SetActive(false);
        if (gameOptions.displayFrameRate) hudFps.gameObject.SetActive(false);
        currentRound = 0;
        lastRoundWinner = 0;
        joinedPlayers = new List<int>();
        playerStats = new Dictionary<int, PlayerStats>();
        kills = new List<Kill>();
        playerSetupUI.GetComponent<PlayerSetupBehavior>().ResetDisplay();
    }
    
    void UpdateHud()
    {
        if (gameOptions.displayFrameRate && !Globals.Instance.paused)
        {
            float msec = (float)Math.Round(deltaTime * 1000.0f, 2);
            float fps = (float)Math.Round(1.0f / deltaTime, 2);
            hudFps.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        }

        TimeSpan t = TimeSpan.FromSeconds(Time.time - roundStartTime);

        string time = string.Format("{0:D2}:{1:D2}:{2:D3}",
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

        hudTime.text = time;
    }

    // Update is called once per frame
    void Update () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        if (isRoundActive)
        {
            List<PlayerController> livingPlayersList = livingPlayers.Values.ToList();
            
            // If we have a living player in multiplayer, or the single player is still alive
            if (livingPlayersList.Count > 1 || (numPlayers == 1 && livingPlayersList.Count > 0))
            {
                UpdateHud();
            }
            // 
            else if (livingPlayersList.Count == 1)
            {
                if (numPlayers > 1)
                {
                    //stop, this player won
                    lastRoundWinner = livingPlayersList[0].GetComponent<PlayerInput>().PlayerNum;
                    StartCoroutine(EndRound());
                }
            }
            else
            {
                StartCoroutine(EndRound());
            }
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
        if (!joinedPlayers.Contains(playerNum))
        {
            joinedPlayers.Add(playerNum);
            hudScores[playerNum - 1].gameObject.SetActive(true);
        }
        if (!playerStats.Keys.Contains(playerNum))
        {
            PlayerStats ps = new PlayerStats();
            ps.playerNum = playerNum;
            playerStats.Add(playerNum, ps);
        }
    }

    public void RemovePlayer(int playerNum)
    {
        if (joinedPlayers.Contains(playerNum)) { joinedPlayers.Remove(playerNum); }
        hudScores[playerNum - 1].gameObject.SetActive(false);
        if (playerStats.ContainsKey(playerNum)) { playerStats.Remove(playerNum); }
    }

    public void DisplayOptions()
    {
        playerSetupUI.SetActive(false);
        optionsUI.gameObject.GetComponent<OptionsScreenBehavior>().DisplayOptions();
    }

    public void DisplayPlayerSetup()
    {
        playerSetupUI.SetActive(true);
        optionsUI.SetActive(false);
    }

    public void StartRound()
    {
        hudTime.gameObject.SetActive(true);
        if (gameOptions.displayFrameRate) hudFps.gameObject.SetActive(true);

        startRoundSound.PlayEffect();
        currentRound++;
        hudRound.text = "Round " + currentRound.ToString();
        hudRound.gameObject.SetActive(true);

        livingPlayers = new Dictionary<int, PlayerController>();
        spinners = new List<GameObject>();
        lasers = new List<GameObject>();
        wallBlades = new List<GameObject>();
        numPlayers = joinedPlayers.Count;

        playerSetupUI.SetActive(false);
        scoreScreenUI.gameObject.SetActive(false);
        roundStartTime = Time.time;

        foreach (int playerNum in joinedPlayers)
        {
            GameObject player = GameObject.Instantiate(playerPrefab, dynamicsParent);
            player.transform.position = playerSpawnPoints[playerNum-1].position;
            livingPlayers[playerNum] = player.GetComponent<PlayerController>();
            livingPlayers[playerNum].GetComponent<PlayerInput>().PlayerNum = playerNum; //todo: clean this up
            SpriteRenderer psr = player.GetComponent<PlayerInput>().sprite.GetComponent<SpriteRenderer>();
            SpriteRenderer ssr = player.GetComponent<PlayerInput>().shield.GetComponent<SpriteRenderer>();

            switch (playerNum)
            {
                case 1:
                    psr.color = player1Color;
                    //ssr.color = player1Color;
                    break;
                case 2:
                    psr.color = player2Color;
                    //ssr.color = player2Color;
                    break;
                case 3:
                    psr.color = player3Color;
                    //ssr.color = player3Color;
                    break;
                case 4:
                    psr.color = player4Color;
                    //ssr.color = player4Color;
                    break;
            }
        }

        if (gameOptions.isSpinnerEnabled)
        {
            if (gameOptions.weird)
            {
                gameOptions.spinnerCount = 3;
                for (int i = 0; i < gameOptions.spinnerCount; i++)
                {
                    GameObject spinner = GameObject.Instantiate(weirdSpinnerPrefabs[i], dynamicsParent);
                    spinner.transform.position = new Vector2(UnityEngine.Random.Range(-3.0f, 3.0f), UnityEngine.Random.Range(-1.5f, 1.5f));
                    spinners.Add(spinner);
                }
            }
            else {
                for (int i = 0; i < gameOptions.spinnerCount; i++)
                {
                    GameObject spinner = GameObject.Instantiate(spinnerPrefab, dynamicsParent);
                    spinner.transform.position = new Vector2(UnityEngine.Random.Range(-3.0f, 3.0f), UnityEngine.Random.Range(-1.5f, 1.5f));
                    spinners.Add(spinner);
                }
            }
        }

        if (gameOptions.isBombEnabled)
        {
            GameObject bombSpawner = GameObject.Instantiate(bombSpawnerPrefab, dynamicsParent);
            bombSpawner.transform.position = new Vector2(0.0f, 9.0f);
        }

        if (gameOptions.isMineEnabled)
        {
            GameObject mineSpawner = GameObject.Instantiate(mineSpawnerPrefab, dynamicsParent);
            mineSpawner.transform.position = new Vector2(0.0f, 0.0f);
        }

        if (gameOptions.isWallLaserEnabled)
        {
            for (int i = 0; i < gameOptions.wallLaserCount; i++)
            {
                CreateWallLaser();
            }
        }

        if (gameOptions.isLaserLineEnabled)
        {
            for (int i = 0; i < gameOptions.laserLineCount; i++)
            {
                GameObject laserLine = GameObject.Instantiate(laserLinePrefab, dynamicsParent);
                int x = UnityEngine.Random.Range(-3, 3);
                laserLine.transform.position = new Vector2(x, 0);
            }
        }

        if (gameOptions.isFloorEnabled)
        {
            tileManager.gameObject.SetActive(true);
            tileManager.Reset();
            tileManager.StartCollapsing();
        }

        // Wall Blade not implemented
        //if (false)
        //{
        //    GameObject wallBlade = GameObject.Instantiate(wallBladePrefab, dynamicsParent);
        //    wallBlade.transform.position = Vector2.zero;
        //    wallBlades.Add(wallBlade);
        //}

        GameObject crateSpawner = GameObject.Instantiate(crateSpawnerPrefab, dynamicsParent);
        crateSpawner.GetComponent<CrateSpawner>().SpawnCrates();

        isRoundActive = true;
        isRoundReady = false;
    }

    void CreateWallLaser()
    {
        // generate Wall Laser
        // N = 0, E = 1, S = 2, W = 3
        int wall = UnityEngine.Random.Range(0, 4);

        Vector2 laserpos = new Vector2();
        Vector2 facing = new Vector2();
        float laserrotation = 0.0f;
        bool IsVertical = false;

        switch (wall)
        {
            case 0:
                laserpos.x = UnityEngine.Random.Range(minWallLaserSpawn.x, maxWallLaserSpawn.x);
                laserpos.y = 7.5f;
                laserrotation = 90.0f;
                facing.x = 0.0f;
                facing.y = -1.0f;
                break;
            case 1:
                laserpos.x = 14.5f;
                laserpos.y = UnityEngine.Random.Range(minWallLaserSpawn.y, maxWallLaserSpawn.y);
                laserrotation = 0.0f;
                facing.x = -1.0f;
                facing.y = 0.0f;
                IsVertical = true;
                break;
            case 2:
                laserpos.x = UnityEngine.Random.Range(minWallLaserSpawn.x, maxWallLaserSpawn.x);
                laserpos.y = -7.5f;
                laserrotation = -90.0f;
                facing.x = 0.0f;
                facing.y = 1.0f;
                break;
            case 3:
                laserpos.x = -14.5f;
                laserpos.y = UnityEngine.Random.Range(minWallLaserSpawn.y, maxWallLaserSpawn.y);
                laserrotation = 180.0f;
                facing.x = 1.0f;
                facing.y = 0.0f;
                IsVertical = true;
                break;
        }

        GameObject wallLaser = GameObject.Instantiate(laserPrefab, dynamicsParent);
        wallLaser.transform.position = laserpos;
        wallLaser.transform.eulerAngles = new Vector3(0, 0, laserrotation);
        WallLaserBehavior wlb = wallLaser.GetComponent<WallLaserBehavior>();
        wlb.shotFrequency = gameOptions.wallLaserShotFrequency;
        wlb.facing = facing;
        wlb.IsVertical = IsVertical;

        lasers.Add(wallLaser);
    }

    public void AddKill(int killer, int victim, Kill.Weapon weapon)
    {
        Kill kill = new Kill(killer, victim, weapon);

        playerStats[killer].kills.Add(kill);
        playerStats[victim].deaths.Add(kill);
        kills.Add(kill);

        if (killer != victim)
        {
            livingPlayers[killer].NotifyOfKill();
        }
    }

    IEnumerator EndRound()
    {
        isRoundActive = false;
        Time.timeScale = 0.0f;

        // Last round ended in tie
        if (lastRoundWinner != 0)
        {
            playerStats[lastRoundWinner].wins++;
            hudScores[lastRoundWinner - 1].text = "Player " + lastRoundWinner + ":  " + playerStats[lastRoundWinner].wins;
        }

        yield return new WaitForSecondsRealtime(2.0f);

        CleanupRound();

        Time.timeScale = 1.0f;
        endRoundSound.PlayEffect();
        scoreScreenUI.Display();
    }

    private void CleanupRound()
    {
        foreach (PlayerController player in livingPlayers.Values)
        {
            if (player != null)
            {
                playerStats[player.GetComponent<PlayerInput>().PlayerNum].survivalTime += Time.time - roundStartTime;
                Destroy(player.gameObject);
            }
        }

        foreach (Transform trans in dynamicsParent)
        {
            Destroy(trans.gameObject);
        }

        tileManager.Reset();
    }

    public PlayerStats GetWinner()
    {
        PlayerStats winner = null;

        foreach (PlayerStats ps in playerStats.Values)
        {
            if (ps.wins >= gameOptions.roundsToWin)
            {
                winner = ps;
            }
        }

        return winner;
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
