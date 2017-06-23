using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderEffectHandler : MonoBehaviour {

    public List<GameEffect> onCollisionEffects;
    public bool isTriggerCollider = false;
    public bool destroyAfterActivation = false;
    public float destroyDelay = 0.0f;

    public bool onlyActivateOnce = true;
    public bool alreadyActivated = false;

    public float delayBetweenActivations = 0.0f;
    float lastActivation = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggerCollider)
        {
            ActivateEffects(collision.collider);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       if (isTriggerCollider)
        {
            ActivateEffects(other);
        }
    }

    void ActivateEffects(Collider2D other)
    {
        if (onlyActivateOnce && alreadyActivated) { return; }

        if (delayBetweenActivations > 0.0f)
        {
            if (Time.time < lastActivation + delayBetweenActivations)
            {
                return;
            }
            lastActivation = Time.time;
        }

        alreadyActivated = true;

        foreach (GameEffect effect in onCollisionEffects)
        {
            effect.ActivateEffect(gameObject, 0.0f, null, other);
        }

        if (destroyAfterActivation)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}
