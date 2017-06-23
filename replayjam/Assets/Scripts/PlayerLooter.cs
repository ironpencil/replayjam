using UnityEngine;
using System.Collections;

public class PlayerLooter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("PlayerLooter:CollisionEnter");
        Collider2D other = coll.collider;

        Lootable lootable = other.gameObject.GetComponent<Lootable>();

        if (lootable != null)
        {
            //pick up the item
            Collider2D thisCollider = gameObject.GetComponent<Collider2D>();

            Debug.Log("PlayerLooter:Collided with loot");
            lootable.PickUp(gameObject, coll, thisCollider);

        }
    }
}
