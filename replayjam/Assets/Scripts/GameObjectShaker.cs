using UnityEngine;
using System.Collections;

public class GameObjectShaker : MonoBehaviour {

    public float magnitude = 1.0f;
    public float sustainTime = 0.2f;
    public float decayTime = 0.2f;

    public bool shakeOnStart = false;
    public bool shakeOnCollision = false;
    public LayerMask collisionMask;

    public GameObjectShake shakeObject;
	// Use this for initialization
	void Start () {
        if (shakeOnStart)
        {
            Shake();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shake()
    {
        if (shakeObject != null)
        {
            shakeObject.ShakeObject(magnitude, sustainTime, decayTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (shakeOnCollision)
        {
            int layer = collision.gameObject.layer;
            if (collisionMask == (collisionMask | (1 << layer)))
            {
                Shake();
            }
        }
    }
}
