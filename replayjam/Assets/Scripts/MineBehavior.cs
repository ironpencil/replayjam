using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : MonoBehaviour, Explosive {
    public Color unarmedColor;
    public Color armedColor1;
    public Color armedColor2;
    public float armedTime;
    public AnimationCurve blinkFrequency;
    public float startFrequency;
    public GameObject explosionPrefab;
    public bool onlyArmByPlayer = true;
    public bool bodyCollisionKillsPlayer = true;
    public MineSpawnerBehavior mineSpawnerBehavior;
    public bool isMineDestroyedOnExplosion = false;
    public float maxLifetime = 0.0f;

    public SoundEffectHandler beepSound;
    public AudioSource slideSource;
    public float slideMaxVolume = 1.0f;

    private float elapsedTime;
    private float elapsedArmedTime;
    private float nextColorSwitch;
    private bool isColor1 = true;
    public bool isArmed = false;
    private List<int> colliders;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().color = unarmedColor;
        colliders = new List<int>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;

        if (maxLifetime > 0 && elapsedTime > maxLifetime) isArmed = true;

        if (isArmed)
        {
            // Increase time spent blinking
            elapsedArmedTime += Time.deltaTime;
            
            if (elapsedArmedTime > nextColorSwitch)
            {
                //e + (.5 - (easing * .5))

                nextColorSwitch = elapsedArmedTime + (startFrequency - (blinkFrequency.Evaluate(elapsedArmedTime / armedTime) * startFrequency));
                isColor1 = !isColor1;
                if (!isColor1) { beepSound.PlayEffect(); }
            }

            if (isColor1)
            {
                gameObject.GetComponent<SpriteRenderer>().color = armedColor1;
            } else
            {
                gameObject.GetComponent<SpriteRenderer>().color = armedColor2;
            }

            if (elapsedArmedTime > armedTime)
            {
                Explode();
            }
        } else
        {
            gameObject.GetComponent<SpriteRenderer>().color = unarmedColor;
        }
    }

    void FixedUpdate()
    {
        slideSource.volume = slideMaxVolume * Mathf.Min((rb2d.velocity.sqrMagnitude / 128), 1.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collided with a player?
        PlayerInput pi = collision.collider.gameObject.GetComponent<PlayerInput>();
        
        if (pi == null)
        {
            try
            {
                // Collided with a player's shield?
                pi = collision.collider.transform.parent.parent.gameObject.GetComponent<PlayerInput>();
            } catch (NullReferenceException)
            {
                // Ignore, mine collided with something other than a player
            }
        } else if (bodyCollisionKillsPlayer)
        {
            Explode();
        }

        if (pi != null)
        {
            colliders.Add(pi.PlayerNum);
        }

        if (!onlyArmByPlayer)
        {
            isArmed = true;
        } else if (pi != null)
        {
            isArmed = true;
        }
    }

    void Explode()
    {
        Transform dynamicsParent = Globals.Instance.GetComponent<GameManager>().dynamicsParent;
        GameObject explosion = Instantiate(explosionPrefab, dynamicsParent);
        explosion.transform.position = transform.position;
        ExplosionBehavior eb = explosion.GetComponent<ExplosionBehavior>();
        eb.explosives.Add(this);
        isArmed = false;
        nextColorSwitch = 0.0f;
        elapsedArmedTime = 0.0f;

        // Remove the mine from visibility for now
        // Destroy gameObject when explosion finishes
        if (isMineDestroyedOnExplosion)
        {
            gameObject.SetActive(false);
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

    public void KilledPlayer(int playerNum)
    {
        int killer = GetLastCollider(playerNum);
        Globals.Instance.GetComponent<GameManager>().AddKill(killer, playerNum, Kill.Weapon.Mine);
    }

    public void StartExploding()
    {
        // Do nothing
    }

    public void EndExploding()
    {
        colliders.Clear();
        if (isMineDestroyedOnExplosion)
        {
            mineSpawnerBehavior.RemoveMine(gameObject);
            Destroy(gameObject);
        }
    }
}
