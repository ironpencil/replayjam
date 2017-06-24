using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour {

    public GameObject projectilePrefab;
    public GameObject shooter;

    public float shootForce = 10.0f;

    public float shootInterval = 0.5f;

    public Vector2 spawnOffset;

    float lastShot = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
		
        //if (Time.time > lastShot + shootInterval)
        //{
        //    Shoot();
        //    lastShot = Time.time;
        //}
	}

    public void Shoot()
    {
        if (Time.time > lastShot + shootInterval)
        {
            GameObject bullet = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            bullet.transform.parent = transform;

            bullet.transform.localPosition = spawnOffset;

            bullet.transform.parent = Globals.Instance.dynamicsParent;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.shooter = shooter;

            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

            bulletRB.AddForce(transform.up * shootForce, ForceMode2D.Impulse);

            GameObject.Destroy(bullet, 10.0f);

            lastShot = Time.time;
        }
    }
}
