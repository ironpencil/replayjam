using UnityEngine;
using System.Collections;

public class LoopedMovement : MonoBehaviour {

    public Vector2 movement = new Vector2(4.0f, 0.0f);

    public Vector2 resetAtPosition = new Vector2(200.0f, 0.0f);

    public Vector2 resetToPosition = Vector2.zero;

    public bool resetHorizontal = true;
    public bool resetVertical = false;

    public RectTransform rect;
	// Use this for initialization
	void Start () {
        if (rect == null)
        {
            rect = gameObject.GetComponent<RectTransform>();
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (rect != null)
        {
            if (resetHorizontal && rect.anchoredPosition.x >= resetAtPosition.x)
            {
                rect.anchoredPosition = resetToPosition;
            }

            if (resetVertical && rect.anchoredPosition.y >= resetAtPosition.y)
            {
                rect.anchoredPosition = resetToPosition;
            }

            rect.anchoredPosition += (movement * Time.deltaTime);
            
        }
        
	
	}
}
