using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour {

    public GameObject projectilePrefab;

    public float shootForce = 10.0f;

    public float shootInterval = 0.5f;

    float lastShot = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
		
        if (Time.time > lastShot + shootInterval)
        {
            Shoot();
            lastShot = Time.time;
        }
	}

    public void Shoot()
    {
        GameObject bullet = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        bulletRB.AddForce(transform.up * shootForce, ForceMode2D.Impulse);

        GameObject.Destroy(bullet, 10.0f);
    }
}
