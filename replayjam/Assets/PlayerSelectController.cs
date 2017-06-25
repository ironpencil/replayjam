using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectController : MonoBehaviour {

    public Text startGame;
    public List<PlayerSelector> playerSelectors;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetScreen()
    {
        foreach (PlayerSelector ps in playerSelectors)
        {
            ps.Leave();
        }
    }
}
