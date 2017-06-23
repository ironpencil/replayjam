using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour {    

    public List<GameObject> monsterPrefabs;

    public Transform enemyParent;

	// Use this for initialization
	void Start () {

        if (monsterPrefabs != null && monsterPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, monsterPrefabs.Count);

            GameObject monster = (GameObject)GameObject.Instantiate(monsterPrefabs[randomIndex], transform.position, Quaternion.identity);

            if (enemyParent == null)
            {
                enemyParent = transform.parent;
            }

            monster.transform.parent = enemyParent;

        }

        //LootHandler monsterLoot = monster.GetComponent<LootHandler>();

        //if (monsterLoot != null)
        //{
        //    monsterLoot.DetermineLoot();
        //}
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
