using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public GameObject tilePrefab;

    public Vector2 arenaSize = new Vector2(32, 18);
    public Vector2 arenaUpperLeft = new Vector2(-15.5f, 8.5f);

    List<CollapsingFloor> tiles;
    public List<Sprite> floorSprites;

    public bool isCollapsing = false;

    public float collapseTime = 1.0f;
    float lastCollapse = 0.0f;
    float actualCollapseTime;
    public int timeGroupSize = 16;
    public float timeGroupAdjust = 0.5f;
    

    int tilesCollapsedInGroup = 0;

	// Use this for initialization
	void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {

        if (isCollapsing && tiles.Count > 0 && Time.time - lastCollapse > actualCollapseTime)
        {
            int tileIndex = Random.Range(0, tiles.Count);
            CollapsingFloor tile = tiles[tileIndex];
            tiles.RemoveAt(tileIndex);
            tile.Collapse();
            tilesCollapsedInGroup++;

            if (tilesCollapsedInGroup >= timeGroupSize)
            {
                actualCollapseTime += timeGroupAdjust;
                tilesCollapsedInGroup = 0;
            }

            lastCollapse = Time.time;
            
        }
		
	}

    public void StartCollapsing()
    {
        isCollapsing = true;
        lastCollapse = Time.time;
    }

    public void Reset()
    {
        isCollapsing = false;
        actualCollapseTime = Globals.Instance.GameManager.gameOptions.floorCollapseInterval;
        //clear any children we may have from a previous execution
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        tiles = new List<CollapsingFloor>();

        for (int x = 0; x < arenaSize.x; x++)
        {
            float tileX = arenaUpperLeft.x + x;
            for (int y = 0; y < arenaSize.y; y++)
            {
                float tileY = arenaUpperLeft.y - y;

                GameObject newTile = GameObject.Instantiate(tilePrefab, transform);
                newTile.transform.position = new Vector2(tileX, tileY);
                CollapsingFloor floorTile = newTile.GetComponent<CollapsingFloor>();
                floorTile.floorImage = floorSprites[Random.Range(0, 4)];
                tiles.Add(floorTile);
            }
        }
    }
}
