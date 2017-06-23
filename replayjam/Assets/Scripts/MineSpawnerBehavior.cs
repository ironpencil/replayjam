using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineSpawnerBehavior : MonoBehaviour {
    public GameObject minePrefab;
    public Vector2 upperSpawnBounds;
    public Vector2 lowerSpawnBounds;
    public int maxMines;
    public float respawnFrequency;
    private float nextRespawnTime;
    public int mineCount;

    private GameManager gm;

    // Use this for initialization
    void Start () {
        gm = Globals.Instance.gameObject.GetComponent<GameManager>();
        maxMines = gm.gameOptions.maxMines;
        respawnFrequency = gm.gameOptions.mineRespawnFrequency;

        while (mineCount < maxMines)
        {
            AddMine();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (mineCount < maxMines)
        {
            if (nextRespawnTime < Time.time)
            {
                AddMine();
            }
        }
	}

    public void AddMine()
    {
        List<PlayerController> players = Globals.Instance.GameManager.livingPlayers.Values.ToList();
        
        GameObject mine = GameObject.Instantiate(minePrefab, gm.dynamicsParent);

        bool mineOnPlayer = true;

        while (mineOnPlayer)
        {
            float x = UnityEngine.Random.Range(lowerSpawnBounds.x, upperSpawnBounds.x + 1);
            float y = UnityEngine.Random.Range(lowerSpawnBounds.y, upperSpawnBounds.y + 1);

            Vector2 minePos = new Vector2(x, y);

            if (!players.Any(p => Vector2.Distance(p.transform.position, minePos) < 2.0f))
            {
                mineOnPlayer = false;
            }

            mine.transform.position = new Vector2(x, y);
        }

        MineBehavior mb = mine.GetComponent<MineBehavior>();
        mb.mineSpawnerBehavior = this;
        mb.isArmed = Globals.Instance.GameManager.gameOptions.mineStartsArmed;
        mb.armedTime = Globals.Instance.GameManager.gameOptions.mineTimeToDetonate;
        mb.maxLifetime = Globals.Instance.GameManager.gameOptions.mineMaxLifetime;

        mineCount++;
        
        if (mineCount < maxMines)
        {
            nextRespawnTime = Time.time + respawnFrequency;
        }
    }

    public void RemoveMine(GameObject Mine)
    {
        mineCount--;
        nextRespawnTime = Time.time + respawnFrequency;
    }
}
