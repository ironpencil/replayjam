using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class VictoryScreenBehavior : MonoBehaviour {
    public Text victoryMessage;

    public Image bgMask;
    public Image textGlow;
    public Image character;
    public Image characterMask;

    public List<Sprite> characters;
    public List<Sprite> characterMasks;

    public void Display()
    {
        GameManager gm = Globals.Instance.GetComponent<GameManager>();
        PlayerStats winner = gm.GetWinner();
        victoryMessage.text = "Player " + winner.playerNum;
        gameObject.SetActive(true);

        Color playerColor = gm.GetPlayerColor(winner.playerNum);

        bgMask.color = playerColor;
        textGlow.color = playerColor;
        characterMask.color = playerColor;

        //just use player num to determine character for now
        character.sprite = characters[winner.playerNum - 1];
        characterMask.sprite = characterMasks[winner.playerNum - 1];
    }


    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 1; i <= 4; i++)
        {
            if (XCI.GetButtonDown(XboxButton.A, (XboxController)i))
            {
                gameObject.SetActive(false);
                Globals.Instance.GameManager.SetupGame();
            }
        }
    }
}
