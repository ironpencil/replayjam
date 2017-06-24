using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject shooter;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Collide(GameObject go)
    {
        PlayerInput player = go.GetComponent<PlayerInput>();

        if (player != null && go != shooter)
        {
            player.Hit();
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collide(collision.gameObject);
    }
}
