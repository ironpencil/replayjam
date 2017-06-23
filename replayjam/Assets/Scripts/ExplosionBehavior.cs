using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour {
    public float lifetime;
    public List<Explosive> explosives;

    public ExplosionBehavior()
    {
        explosives = new List<Explosive>();
    }

	// Use this for initialization
	void Start () {
        foreach (Explosive ex in explosives)
        {
            ex.StartExploding();
        }
    }
    
	// Update is called once per frame
	void Update () {
		if (lifetime <= 0)
        {
            foreach (Explosive ex in explosives)
            {
                ex.EndExploding();
            }
            Destroy(gameObject);
        } else
        {
            lifetime -= Time.deltaTime;
        }
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerController pc = collider.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            foreach (Explosive ex in explosives)
            {
                ex.KilledPlayer(pc.gameObject.GetComponent<PlayerInput>().PlayerNum);
            }
            pc.Kill(PlayerController.KillType.Explode);
        }
    }
}
