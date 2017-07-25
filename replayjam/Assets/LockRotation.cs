using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {

    public float lockedRotation = 0.0f;
	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, lockedRotation));
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, lockedRotation));
    }
}
