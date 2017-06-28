using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class RoundWonBehavior : MonoBehaviour {

    private GameManager gm;

    public List<Text> playerNames;
    public List<Text> playerScores;
    public List<GameObject> playerKillCounts;

    public GameObject killIconPrefab;

    public Text winnerText;

    public float killStampDelay = 1.0f;
    public float killStampInterval = 0.2f;

    public SoundEffectHandler killStampSound;
    private int soundsPlayed = 0;

	// Use this for initialization
	void Start () {
        gm = Globals.Instance.GameManager;
	}
	
	// Update is called once per frame
	void Update () {
        //UpdateFields();

        for (int i = 1; i <= 4; i++)
        {
            if (XCI.GetButtonDown(XboxButton.A, (XboxController)i))
            {
                if (gm.gameMode == GameManager.GameMode.Deathmatch || gm.lastRoundWinner.roundsWon > 2)
                {
                    gm.EndGame();
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(false);
                    gm.StartRound();
                }
            }
        }
    }

    public void DisplayScreen()
    {
        gameObject.SetActive(true);
        UpdateFields();        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateFields()
    {
        if (gm == null) gm = Globals.Instance.GameManager;
        winnerText.text = gm.lastRoundWinner.name + " Wins the Round!";
        winnerText.color = gm.GetPlayerColor(gm.lastRoundWinner.playerNum);

        foreach (Text t in playerScores)
        {
            t.transform.parent.parent.gameObject.SetActive(false);
        }

        foreach (GameObject killCount in playerKillCounts)
        {
            foreach (Transform child in killCount.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        int i = 0;

        GameManager.GameMode gameMode = gm.gameMode;
        soundsPlayed = 0;

        foreach (PlayerInfo pi in gm.joinedPlayers)
        {
            playerNames[i].transform.parent.parent.gameObject.SetActive(true);

            Color playerColor = gm.GetPlayerColor(pi.playerNum);
            playerNames[i].color = playerColor;
            playerNames[i].text = pi.name;

            playerScores[i].color = playerColor;
            if (gameMode == GameManager.GameMode.Survival)
            {
                playerScores[i].text = pi.roundsWon + "";
                playerKillCounts[i].SetActive(false);
            }
            else if (gameMode == GameManager.GameMode.Deathmatch)
            {
                int kills = 0;
                List<int> playerKills;

                GameObject kc = playerKillCounts[i];
                kc.SetActive(true);

                if (gm.kills.TryGetValue(pi.playerNum, out playerKills))
                {
                    //display all kill icons
                    kills = playerKills.Count;

                    StartCoroutine(AddKills(playerScores[i], kc, playerKills));
                }
                
                //set score to kills
                //playerScores[i].text = kills.ToString();
            }

            i++;
        }
    }

    private IEnumerator AddKills(Text playerScore, GameObject kc, List<int> kills)
    {
        playerScore.text = "0";

        yield return new WaitForSeconds(killStampDelay);

        int killCount = 0;

        foreach (int kill in kills)
        {
            killCount++;
            playerScore.text = killCount.ToString();

            //only play the kill sound once per kill instead of 4x at once for 4 players
            if (killCount > soundsPlayed)
            {
                if (killStampSound != null) { killStampSound.PlayEffect(); }
                soundsPlayed++;
            }

            GameObject killIcon = GameObject.Instantiate(killIconPrefab, kc.transform);
            Image killImage = killIcon.GetComponent<Image>();
            killImage.sprite = Globals.Instance.GameManager.GetPlayerKillIcon(kill);

            yield return new WaitForSeconds(killStampInterval);
        }
    }
}
