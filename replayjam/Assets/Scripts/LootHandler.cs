using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootHandler : MonoBehaviour {

    public List<GameObject> lootTablePrefabs;
    public List<float> lootTableChances;

    public List<GameObject> carrying;

    public Transform lootParent;

    public Vector2 scatterRange = new Vector2(6, 4);

    public float scatterForce = 10.0f;

    public bool doScatter = true;


	// Use this for initialization
	void Start () {
        DetermineLoot();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DetermineLoot()
    {
        for (int i = 0; i < lootTablePrefabs.Count; i++)
        {
            float randomChance = 0.0f;
            try
            {
                randomChance = lootTableChances[i];
            }
            catch { }

            if (Random.Range(0.0f, 1.0f) < randomChance)
            {
                //we're carrying this loot
                GameObject lootPrefab = lootTablePrefabs[i];
                if (lootPrefab != null) {
                    GameObject lootItem = (GameObject)GameObject.Instantiate(lootPrefab, transform.position, transform.rotation);
                    lootItem.SetActive(false);
                    lootItem.transform.parent = transform;

                    carrying.Add(lootItem);
                }
            }
        }
    }

    public void DropLoot()
    {
        if (lootParent == null)
        {
            lootParent = transform.parent;
        }

        bool scatterTemp = doScatter;

        //if (carrying.Count < 2) { doScatter = false; }

        foreach (GameObject lootItem in carrying)
        {
            DropLootItem(lootItem);
        }

        doScatter = scatterTemp;
    }

    private void DropLootItem(GameObject lootItem)
    {
        Vector2 dropPos = new Vector2(transform.position.x, transform.position.y);        
        
        //GameObject drop = (GameObject)GameObject.Instantiate(lootItem, dropPos, transform.rotation);

        lootItem.transform.position = dropPos;        

        lootItem.transform.parent = lootParent;

        lootItem.SetActive(true);

        if (doScatter)
        {
            Rigidbody2D lootRB = lootItem.GetComponent<Rigidbody2D>();

            int randomPosX = Random.Range((int)scatterRange.x * -1, (int)scatterRange.x + 1);
            int randomPosY = Random.Range((int)scatterRange.y * -1, (int)scatterRange.y + 1);

            dropPos.x += randomPosX;
            dropPos.y += randomPosY;

            lootRB.AddRelativeForce(new Vector2(randomPosX, randomPosY) * scatterForce, ForceMode2D.Impulse);
        }
    }
}
