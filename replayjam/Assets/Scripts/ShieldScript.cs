using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour {
    Vector2 offset;
    Vector2 resetPos;
    Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        //resetPos = transform.localPosition;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        offset = transform.localPosition;
	}
	
	void FixedUpdate () {
        //transform.localPosition = resetPos;
        //transform.eulerAngles = Vector3.zero;
        rb2d.position = (Vector2)transform.parent.position + offset;
        //rb2d.rotation = 0;
	}
}
