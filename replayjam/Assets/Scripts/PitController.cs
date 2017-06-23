using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitController : MonoBehaviour {

    public Collider2D pitCollider;
    SpriteRenderer pitSprite;
	// Use this for initialization
	void Start () {
        pitSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        int randomRotation = Random.Range(0, 4) * 90;
        Vector3 newRotation = pitSprite.transform.eulerAngles;
        newRotation.z = randomRotation;
        pitSprite.transform.eulerAngles = newRotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivatePit(bool active)
    {
        pitCollider.enabled = active;
    }
}
