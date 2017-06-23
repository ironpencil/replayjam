using UnityEngine;
using System.Collections;

public class UpdateSortingLayer : MonoBehaviour {

    public SpriteRenderer sprite;

    public float yOffset = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        sprite.sortingOrder = (int) (sprite.transform.position.y + yOffset) * -1;
	
	}
}
