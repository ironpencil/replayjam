using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class VictoryBehavior : MonoBehaviour {
    public Text victoryText;
    public GameManager gm;

	// Use this for initialization
	void Start () {
        gm = Globals.Instance.GameManager;
	}
	
	// Update is called once per frame
	void Update () {
        
        victoryText.text = gm.lastRoundWinner.name + " is the victor!";
        victoryText.color = gm.GetPlayerColor(gm.lastRoundWinner.playerNum);

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
