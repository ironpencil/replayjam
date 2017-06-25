using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class VictoryBehavior : MonoBehaviour {
    public Text victoryText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        victoryText.text = Globals.Instance.GameManager.lastRoundWinner.name + " is the victor!";

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
