using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillKountKontroller : MonoBehaviour {

    public int playerNum;
    public List<int> kills;

    public GameObject killPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddKill(int playerNum)
    {
        kills.Add(playerNum);

        GameObject icon = GameObject.Instantiate(killPrefab, transform);
        Image iconImage = icon.GetComponent<Image>();
        iconImage.sprite = Globals.Instance.GameManager.GetPlayerKillIcon(playerNum);
    }

    public void Reset()
    {
        kills.Clear();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
