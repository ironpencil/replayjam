using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioEnforcer : MonoBehaviour {

    public Vector2 aspectRatio = new Vector2(16, 9);

    Vector2 prevScreenSize = new Vector2();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int x = Screen.width;
        int y = Screen.height;
        
        if (prevScreenSize.x != x || prevScreenSize.y != y)
        {
            //screen was resized, force to aspect
            float targetX = y * (aspectRatio.x / aspectRatio.y);
            float targetY = x * (aspectRatio.y / aspectRatio.x);

            if (x > targetX)
            {
                //x has exceeded ratio - clamp it
                x = (int)targetX;
                y = (int)(x * (aspectRatio.y / aspectRatio.x));
                
            } else if (y > targetY)
            {
                //y has exceeded ratio - clamp it
                y = (int)targetY;
                x = (int)(y * (aspectRatio.x / aspectRatio.y));
            }

            Screen.SetResolution(x, y, Screen.fullScreen);            
        }

        prevScreenSize.x = x;
        prevScreenSize.y = y;
	}
}
