using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour {

    public int minCrates = 0;
    public int maxCrates = 6;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    public bool doSpawnCrates = true;

    public List<GameObject> cratePrefabs;

    List<GameObject> spawnedCrates = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnCrates()
    {
        ClearCrates();

        int numCrates = Random.Range(minCrates, maxCrates + 1);

        for (int i = 0; i < numCrates; i++)
        {
            SpawnCrate();
        }
    }

    private void SpawnCrate()
    {
        int randomCrate = Random.Range(0, cratePrefabs.Count);
        GameObject crate = GameObject.Instantiate(cratePrefabs[randomCrate], Globals.Instance.GameManager.dynamicsParent);
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        float randomRotation = Random.Range(0.0f, 360.0f);

        crate.transform.position = new Vector3(randomX, randomY);
        crate.transform.eulerAngles = new Vector3(0.0f, 0.0f, randomRotation);

        spawnedCrates.Add(crate);
    }

    public void ClearCrates()
    {
        foreach (GameObject crate in spawnedCrates)
        {
            try
            {
                Destroy(crate);
            }
            catch { }
        }

        spawnedCrates.Clear();
    }
}
