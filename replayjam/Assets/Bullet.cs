using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject shooter;

    public TrailRenderer bulletTrail;
    public ParticleSystem bulletParticles;
    
	// Use this for initialization
	void Start () {
        PlayerInput pi = shooter.GetComponent<PlayerInput>();
        Color playerColor = Globals.Instance.GameManager.GetPlayerColor(pi.playerInfo.playerNum);
        bulletTrail.startColor = playerColor;
        bulletTrail.endColor = playerColor;

        var main = bulletParticles.main;
        main.startColor = playerColor;

        //Gradient gradient = bulletTrail.colorGradient;
        //GradientColorKey[] gcks = gradient.colorKeys;

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
