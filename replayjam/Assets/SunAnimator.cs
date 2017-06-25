using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunAnimator : MonoBehaviour {

    Image sunImage;

    public List<Sprite> sunSprites;
    public int spriteIndex = 0;
    public float frameInterval = 0.2f;

    float nextChange = 0.0f;
	// Use this for initialization
	void Start () {
        sunImage = gameObject.GetComponent<Image>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Time.time > nextChange)
        {
            spriteIndex++;

            if (spriteIndex == sunSprites.Count)
            {
                spriteIndex = 0;
            }

            sunImage.sprite = sunSprites[spriteIndex];

            nextChange = Time.time + frameInterval;
        }
	}
}
