using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoundsChecker : MonoBehaviour {

    public List<Transform> bounds;

    List<Collider2D> pitColliders;

    PlayerController playerController;

	// Use this for initialization
	void Start () {
        pitColliders = new List<Collider2D>();
        playerController = gameObject.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        CheckBounds();
        pitColliders.Clear();
    }

    private void CheckBounds()
    {
        //check to see if all of our bound points are overlapped by at least one collider
        bool inPit = bounds.All(p => pitColliders.Any(c => c.OverlapPoint(p.position)));
        
        if (inPit)
        {
            playerController.Kill(PlayerController.KillType.Fall);
        } 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Pit"))
        {
            pitColliders.Add(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals("Pit"))
        {
            pitColliders.Add(other);
        }
    }
}
