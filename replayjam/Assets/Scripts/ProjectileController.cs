using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    public float moveSpeed = 10;
    public Vector2 fireDirection;
    public int maxWallBounceCount = 4;
    public GameObject collisionParticle;
    private int currentWallBounceCount = 0;
    private List<int> colliders;
    
    Rigidbody2D rb2d;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = fireDirection * moveSpeed;
        colliders = new List<int>();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 direction = rb2d.velocity.normalized;
        rb2d.velocity = direction * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        GameObject particle = Instantiate(collisionParticle);
        Vector3 particlePos = transform.position;
        particlePos.z = -1;
        particle.transform.position = particlePos;
        float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg - 45;
        Vector3 rotation = new Vector3(0, 0, angle);
        particle.transform.eulerAngles = rotation;

        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            int playerNum = pc.gameObject.GetComponent<PlayerInput>().PlayerNum;
            Globals.Instance.GameManager.AddKill(GetLastCollider(playerNum), playerNum, Kill.Weapon.Laser);
            pc.Kill(PlayerController.KillType.Dissolve);
            Destroy(gameObject);
        } else if (collision.gameObject.GetComponent<WallBehavior>() != null)
        {
            currentWallBounceCount++;
            if (currentWallBounceCount > maxWallBounceCount)
            {
                Destroy(gameObject);
            }
        } else
        {
            try
            {
                // Collided with a player's shield?
                PlayerInput pi = collision.collider.transform.parent.parent.gameObject.GetComponent<PlayerInput>();
                colliders.Add(pi.PlayerNum);
            }
            catch (NullReferenceException)
            {
                // Ignore, projectile collided with something other than a player
            }
        }
    }

    public int GetLastCollider(int playerNum)
    {
        int lastCollider = playerNum;

        for (int i = colliders.Count - 1; i >= 0; i--)
        {
            lastCollider = colliders[i];

            if (colliders[i] != playerNum)
            {
                break;
            }
        }

        return lastCollider;
    }
}
