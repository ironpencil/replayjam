using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitBehavior : MonoBehaviour {
    
    public List<Image> portraits;
    public List<Image> glassStages;

    private RectTransform rt;

    // Slide Variables
    private float startTime;
    public Vector2 startPos;
    public Vector2 endPos;
    public float slideTime;
    public AnimationCurve slideCurve;

    // Use this for initialization
    void Start () {
        startTime = Time.time;
        rt = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        float percent = slideCurve.Evaluate((Time.time - startTime) / slideTime);
        Debug.Log("Percent: " + percent);
        rt.position = Vector2.Lerp(startPos, endPos, percent);
	}
}
