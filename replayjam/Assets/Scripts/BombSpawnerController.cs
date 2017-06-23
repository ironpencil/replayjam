using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombSpawnerController : MonoBehaviour {
    public float frequency = 5;
    public float heightOffset = 20;
    public GameObject BombPrefab;
    public GameObject ShadowPrefab;
    public Vector2 UpperLeftBound;
    public Vector2 LowerRightBound;
    private float timeLeft = 0;

	// Use this for initialization
	void Start () {
        timeLeft = frequency;
        frequency = Globals.Instance.GameManager.gameOptions.bombFrequency;
    }
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            DropBomb();
            timeLeft = frequency;
        }
    }

    void DropBomb()
    {
        //float x = Random.Range(UpperLeftBound.x, LowerRightBound.x);
        //float y = Random.Range(UpperLeftBound.y, LowerRightBound.y);
        List<int> livingPlayerNums = Globals.Instance.GameManager.livingPlayers.Keys.ToList();

        if (livingPlayerNums.Count > 0)
        {
            int targetPlayerNum = livingPlayerNums[Random.Range(0, livingPlayerNums.Count)];
            Globals.Instance.GameManager.playerStats[targetPlayerNum].bombTargets++;
            PlayerController pc = Globals.Instance.GameManager.livingPlayers[targetPlayerNum];

            Transform DynamicsParent = Globals.Instance.GetComponent<GameManager>().dynamicsParent;

            GameObject bombShadow = Instantiate(ShadowPrefab, DynamicsParent);
            bombShadow.transform.position = new Vector2(pc.transform.position.x, pc.transform.position.y);

            GameObject bomb = Instantiate(BombPrefab, DynamicsParent);
            bomb.transform.position = new Vector2(bombShadow.transform.position.x, bombShadow.transform.position.y + heightOffset);

            BombBehavior bb = bomb.GetComponent<BombBehavior>();
            bb.shadow = bombShadow;
            bb.targetPlayerNum = targetPlayerNum;
            bb.FallTime = Globals.Instance.GameManager.gameOptions.bombFallTime;
        }
    }
}
