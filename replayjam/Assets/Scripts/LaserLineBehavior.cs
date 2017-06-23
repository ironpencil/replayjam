using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLineBehavior : MonoBehaviour {
    private GameOptions go;
    private float speed;
    public Rigidbody2D rb2d;
    private Vector2 direction;

	// Use this for initialization
	void Start () {
        go = Globals.Instance.GameManager.gameOptions;
        direction = new Vector2(1, 0);
        speed = Random.Range(go.laserLineMinSpeed, go.laserLineMaxSpeed);
        if (Random.value < 0.5f) speed = -speed;
    }
	
	// Update is called once per frame
	void Update () {
        rb2d.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<WallBehavior>() != null)
        {
            speed = -speed;
        } else
        {
            PlayerController pc = collider.gameObject.GetComponent<PlayerController>();

            if (pc != null && !pc.GetComponent<PlayerInput>().IsDashing)
            {
                pc.Kill(PlayerController.KillType.Dissolve);
            }
        }
    }
}
