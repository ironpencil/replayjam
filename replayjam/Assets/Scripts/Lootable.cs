using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lootable : MonoBehaviour {

    public List<GameEffect> pickupEffects;

    public bool destroyOnPickup = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PickUp(GameObject pickedUpBy, Collision2D coll, Collider2D other)
    {
        foreach (GameEffect gameEffect in pickupEffects)
        {
            gameEffect.ActivateEffect(pickedUpBy, 0.0f, coll, other);
        }

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }
}
